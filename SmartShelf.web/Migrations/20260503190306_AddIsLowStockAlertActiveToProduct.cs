using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartShelf.web.Migrations
{
    /// <inheritdoc />
    public partial class AddIsLowStockAlertActiveToProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsLowStockAlertActive",
                table: "Product",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsLowStockAlertActive",
                table: "Product");
        }
    }
}
