using ProductCatalog.Attributes;
using ProductCatalog.Core;
using System.Collections.Generic;
using System.Text.Json;
namespace ProductCatalog.Midleware
{
    public class ResponseHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ResponseHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var endpoint = context.GetEndpoint();
            var formatResponse = endpoint?.Metadata
                .GetMetadata<ApiResponseAttribute>();

            // Nếu không có attribute hoặc attribute đã tắt, tiếp tục pipeline mà không format response
            if (formatResponse == null || !formatResponse.Enabled)
            {
                await _next(context);
                return;
            }

            // Lưu lại Response Body ban đầu
            var originalBodyStream = context.Response.Body;
            using var newBodyStream = new MemoryStream();
            context.Response.Body = newBodyStream;

            try
            {
                // Tiếp tục pipeline để xử lý request
                await _next(context);

                // Đặt lại con trỏ về đầu stream
                newBodyStream.Seek(0, SeekOrigin.Begin);

                // Đọc nội dung của response
                var responseBody = await new StreamReader(newBodyStream).ReadToEndAsync();
                newBodyStream.Seek(0, SeekOrigin.Begin);

                // Lấy mã status code
                var statusCode = context.Response.StatusCode;

                // Lấy dữ liệu (Data)
                object data = null;
                if (!string.IsNullOrEmpty(responseBody))
                {
                    try
                    {
                        data = JsonSerializer.Deserialize<object>(responseBody);
                    }
                    catch
                    {
                        data = responseBody;
                    }
                }

                // Xử lý các lỗi validation nếu có
                var errorMessages = new List<string>();
                if (data is JsonElement jsonElement && jsonElement.ValueKind == JsonValueKind.Object)
                {
                    bool hasErrors = false;

                    // Check for "errors" property
                    if (jsonElement.TryGetProperty("errors", out var errorsElement))
                    {
                        foreach (var errorProperty in errorsElement.EnumerateObject())
                        {
                            var fieldName = errorProperty.Name;
                            var fieldErrors = errorProperty.Value.EnumerateArray()
                                .Select(error => $"{fieldName}: {error.GetString()}");
                            errorMessages.AddRange(fieldErrors);
                            hasErrors = true; // Mark that we found errors
                        }
                    }

                    // Optionally handle "message" property
                    if (jsonElement.TryGetProperty("message", out var messageElement))
                    {
                        errorMessages.Add(messageElement.GetString());
                    }

                    // Reset data if there were errors
                    if (hasErrors)
                    {
                        data = null; // Resetting data as errors are processed
                    }
                }
                else
                {
                    if ((statusCode < 200 || statusCode >= 300) && data != null)
                    {
                        errorMessages.Add(data.ToString());
                    }
                }
                


                // Tạo đối tượng ResponseApiFormat dựa trên status code
                var apiResponse = new ResponseApiFormat
                {
                    statusCode = statusCode,
                    message = GetStatusMessage(statusCode),
                    data = statusCode >= 200 && statusCode < 300 ? data : null,
                    success = statusCode >= 200 && statusCode < 300,
                    errorMessages = errorMessages.Any() ? errorMessages : null
                };

                // Chuyển đổi đối tượng thành JSON
                var responseJson = JsonSerializer.Serialize(apiResponse);

                // Đặt lại body stream về ban đầu
                context.Response.Body = originalBodyStream;

                // Viết lại response dưới dạng JSON
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(responseJson);
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ nếu có
                var errorResponse = new ResponseApiFormat
                {
                    statusCode = 400,
                    message = "An unexpected error occurred.",
                    data = null,
                    success = false,
                    errorMessages = new List<string> { ex.Message }
                };

                var errorJson = JsonSerializer.Serialize(errorResponse);
                context.Response.Body = originalBodyStream;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(errorJson);
            }
        }

        private string GetStatusMessage(int statusCode)
        {
            return statusCode switch
            {
                >= 200 and < 300 => "Request successful",
                >= 400 and < 500 => "Client error occurred",
                >= 500 and < 600 => "Server error occurred",
                _ => "Request failed"
            };
        }
    }
}
