namespace Video_Media_Processor.Backend.Data.Models
{
    public class Uploads
    {
        public long Id { get; set; }
        public string FilePath { get; set; }
        public UploadStatus Status { get; set; }
        public string Uuid { get; set; }
    }
}
