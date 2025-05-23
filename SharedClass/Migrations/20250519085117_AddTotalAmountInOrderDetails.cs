using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SharedClass.Migrations
{
    /// <inheritdoc />
    public partial class AddTotalAmountInOrderDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "TotalAmount",
                table: "OrderDetails",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "UnitPrice",
                table: "OrderDetails",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalAmount",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "UnitPrice",
                table: "OrderDetails");
        }
    }
}
