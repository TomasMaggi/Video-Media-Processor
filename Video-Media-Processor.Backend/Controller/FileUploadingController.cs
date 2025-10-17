using Microsoft.AspNetCore.Mvc;
using Video_Media_Processor.Backend.Controller.DTOs;
using Video_Media_Processor.Backend.Services;


namespace Video_Media_Processor.Backend.Controller
{
    [ApiController]
    [Route("api/v1/")]
    public class FileUploadingController : ControllerBase
    {
        private readonly IUploadService _uploadService;

        public FileUploadingController(IUploadService uploadService)
        {
            _uploadService = uploadService;
        }

        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<UploadResponse>> UploadMedia([FromForm] UploadRequest request)
        {
            if (request?.media == null || request.media.Length == 0)
                return BadRequest("No media file uploaded");

            if (request.queries == null || !request.queries.Any())
                return BadRequest("No queries provided");

            var res = await _uploadService.HandleUpload(request);

            return Ok(res);
        }
    }
}
