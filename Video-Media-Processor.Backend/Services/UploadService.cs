using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Video_Media_Processor.Backend.Controller.DTOs;
using Video_Media_Processor.Backend.Data;
using Video_Media_Processor.Backend.Data.Models;

namespace Video_Media_Processor.Backend.Services
{
    public class UploadService : IUploadService
    {
        private readonly ApiDataContext _context;

        public UploadService(ApiDataContext dataContext)
        {
            _context = dataContext;
        }

        public async Task<UploadResponse> HandleUpload(UploadRequest uploadRequest)
        {
            var uploadFolder = Path.Combine("C:\\tmp\\");

            if (!Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder);
            }

            var uniqueFileName = Guid.NewGuid().ToString() + "_" + "app1" + uploadRequest.media.FileName;
            var filePath = Path.Combine(uploadFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                uploadRequest.media.CopyTo(fileStream);
            }

            // init new upload
            var upload = new Uploads();
            upload.Uuid = Guid.NewGuid().ToString();
            upload.FilePath = filePath;
            upload.Status = _context.UploadStatus
                                    .First(u => u.Strategy == "pending");

            var options = new JsonSerializerOptions { WriteIndented = true };
            upload.Queries = JsonSerializer.Serialize(uploadRequest.queries.ToList(), options);

            await _context.Uploads.AddAsync(upload);
            await _context.SaveChangesAsync();

            var result = new UploadResponse();
            result.status = "Sucess";
            result.job_uuid = upload.Uuid;

            return result;
        }
    }
}
