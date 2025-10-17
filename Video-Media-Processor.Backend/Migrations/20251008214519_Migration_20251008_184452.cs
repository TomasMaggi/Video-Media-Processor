using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Video_Media_Processor.Backend.Migrations
{
    /// <inheritdoc />
    public partial class Migration_20251008_184452 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Queries",
                table: "Uploads",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Queries",
                table: "Uploads");
        }
    }
}
