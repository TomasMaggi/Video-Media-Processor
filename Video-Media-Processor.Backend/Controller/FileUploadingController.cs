using Microsoft.AspNetCore.Mvc;
using Video_Media_Processor.Backend.Controller.DTOs;


namespace Video_Media_Processor.Backend.Controller
{
    [ApiController]
    [Route("api/v1/")]
    public class FileUploadingController : ControllerBase
    {
        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<UploadResponse>> UploadMedia([FromForm] UploadRequest request)
        {
            string logPath = @"C:\tmp\debug_log.txt";

            string debugInfo = $"Method entered at: {DateTime.Now}\n";
            debugInfo += $"Request is null: {request == null}\n";

            if (request != null)
            {
                debugInfo += $"Media is null: {request.media == null}\n";
                debugInfo += $"Media filename: {request.media?.FileName}\n";
                debugInfo += $"Media length: {request.media?.Length} bytes\n";
                debugInfo += $"Queries is null: {request.queries == null}\n";
                debugInfo += $"Queries count: {request.queries?.Count}\n";
            }

            System.IO.File.WriteAllText(logPath, debugInfo);

            try
            {
                if (request?.media == null || request.media.Length == 0)
                    return BadRequest("No media file uploaded");

                if (request.queries == null || !request.queries.Any())
                    return BadRequest("No queries provided");

                byte[] mediaBytes;
                using (var memoryStream = new MemoryStream())
                {
                    await request.media.CopyToAsync(memoryStream);
                    mediaBytes = memoryStream.ToArray();
                }

                System.IO.File.AppendAllText(logPath, $"SUCCESS: Media: {mediaBytes.Length} bytes, Queries: {string.Join(", ", request.queries)}\n");

                var response = new UploadResponse();
                response.status = "ok";
                response.job_uuid = Guid.NewGuid().ToString();

                return Ok(response);
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText(logPath, $"Exception: {ex.Message}\n");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
