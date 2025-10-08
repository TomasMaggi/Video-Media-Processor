using Microsoft.EntityFrameworkCore;
using Video_Media_Processor.Backend.Data.Models;


namespace Video_Media_Processor.Backend.Data
{
    public class ApiDataContext : DbContext
    {
        public ApiDataContext(DbContextOptions<ApiDataContext> options)
            : base(options)
        {
        }

        public DbSet<Uploads> Uploads { get; set; }
        public DbSet<UploadStatus> UploadStatus { get; set; }
    }
}
