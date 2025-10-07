namespace Video_Media_Processor.Backend.Controller.DTOs
{
    public class UploadRequest
    {
        public IFormFile media { get; set; }
        public IList<string> queries { get; set; }
    }
}