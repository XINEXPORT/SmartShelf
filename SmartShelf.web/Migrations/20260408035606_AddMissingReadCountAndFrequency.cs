using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartShelf.web.Migrations
{
    /// <inheritdoc />
    public partial class AddMissingReadCountAndFrequency : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Frequency",
                table: "TagReadEvent",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ReadCount",
                table: "TagReadEvent",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Frequency",
                table: "TagCurrentState",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ReadCount",
                table: "TagCurrentState",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Frequency",
                table: "TagReadEvent");

            migrationBuilder.DropColumn(
                name: "ReadCount",
                table: "TagReadEvent");

            migrationBuilder.DropColumn(
                name: "Frequency",
                table: "TagCurrentState");

            migrationBuilder.DropColumn(
                name: "ReadCount",
                table: "TagCurrentState");
        }
    }
}
