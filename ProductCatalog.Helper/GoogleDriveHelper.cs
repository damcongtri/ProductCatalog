using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Upload;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace UploadFile
{
    public static class UploadToDrive
    {
        private static readonly string _credentialsPath = "D:/BKAP/sem4/BTL/Credentials.json";
        private static readonly string _applicationName = "GoogleDriveUploadTest";

        //public static UploadToDrive(string credentialsPath, string applicationName)
        //{
        //    _credentialsPath = credentialsPath;
        //    _applicationName = applicationName;
        //}

        // Initialize the Drive service
        private static DriveService InitializeDriveService()
        {
            GoogleCredential credential;
            using (var stream = new FileStream(_credentialsPath, FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream).CreateScoped(new[]
                {
                    DriveService.ScopeConstants.DriveFile
                });
            }

            // Return a new instance of DriveService
            return new DriveService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = _applicationName
            });
        }

        // Async method to upload an IFormFile to Google Drive
        // Async method to upload an IFormFile to Google Drive
        public static async Task<string> UploadFileAsync(IFormFile file, string folderId = "1RlHF94R_hrSxmYkUDkaB8BZoCfGd3Gpx")
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("The file is null or empty.", nameof(file));
            }

            var service = InitializeDriveService();

            var fileMetadata = new Google.Apis.Drive.v3.Data.File
            {
                Name = file.FileName,
                Parents = folderId != null ? new List<string> { folderId } : null
            };

            try
            {
                using (var stream = file.OpenReadStream())
                {
                    var request = service.Files.Create(fileMetadata, stream, file.ContentType);
                    request.Fields = "id";
                    var result = await request.UploadAsync();

                    if (result.Status == UploadStatus.Failed)
                    {
                        throw new Exception($"File upload failed: {result.Exception?.Message}");
                    }

                    var uploadedFile = request.ResponseBody;
                    Console.WriteLine($"File '{fileMetadata.Name}' uploaded with ID: {uploadedFile.Id}");
                    return $"https://lh3.google.com/u/1/d/{uploadedFile.Id}";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during file upload: {ex.Message}");
                throw;
            }
        }
    }
}
