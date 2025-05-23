using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SharedClass.Migrations
{
    /// <inheritdoc />
    public partial class AddFeedbackSubject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Subject",
                table: "FeedBacks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Subject",
                table: "FeedBacks");
        }
    }
}
