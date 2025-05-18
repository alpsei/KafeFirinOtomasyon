using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SharedClass.Migrations
{
    /// <inheritdoc />
    public partial class AddSalary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Salary",
                table: "Users",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Salary",
                table: "Users");
        }
    }
}
