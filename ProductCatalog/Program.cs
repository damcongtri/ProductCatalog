using ProductCatalog.Core;
using ProductCatalog.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using ProductCatalog.Midleware;

var builder = WebApplication.CreateBuilder(args);

// Cấu hình Kestrel để lắng nghe trên địa chỉ IP của mạng nội bộ

//builder.WebHost.ConfigureKestrel(serverOptions =>
//{
//    serverOptions.ListenAnyIP(5000); // Địa chỉ IP và cổng nội bộ
//});
// Thêm dịch vụ CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("MyPolicy", builder =>
    {
        builder.WithOrigins("http://localhost:4200", "http://localhost:3000", "http://192.168.0.106:8089", "*")
            .AllowAnyMethod()
            .AllowAnyHeader();
            //.AllowCredentials();
    });
});

// Đăng ký các dịch vụ cần thiết
builder.RegisterDependencies();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddControllers();

//builder.AddIden
// Cấu hình Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseCors("MyPolicy");
// Sử dụng middleware tùy chỉnh
app.UseMiddleware<ResponseHandlingMiddleware>();

//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseAuthorization();

app.MapControllers();

app.Run(); // Chạy ứng dụng
