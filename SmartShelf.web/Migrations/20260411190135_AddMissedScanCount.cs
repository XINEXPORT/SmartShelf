using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartShelf.web.Migrations
{
    /// <inheritdoc />
    public partial class AddMissedScanCount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MissedScanCount",
                table: "TagCurrentState",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MissedScanCount",
                table: "TagCurrentState");
        }
    }
}
