using Video_Media_Processor.Backend.Controller.DTOs;

namespace Video_Media_Processor.Backend.Services
{
    public interface IUploadService
    {
        Task<UploadResponse> HandleUpload(UploadRequest uploadRequest);
    }
}