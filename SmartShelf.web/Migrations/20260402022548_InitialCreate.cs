using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartShelf.web.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Threshold = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Reader",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reader", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tag",
                columns: table => new
                {
                    EPC = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tag", x => x.EPC);
                    table.ForeignKey(
                        name: "FK_Tag_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TagCurrentState",
                columns: table => new
                {
                    EPC = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ReaderId = table.Column<int>(type: "int", nullable: false),
                    Antenna = table.Column<int>(type: "int", nullable: false),
                    Rssi = table.Column<int>(type: "int", nullable: false),
                    LastSeenTimestamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TagCurrentState", x => x.EPC);
                    table.ForeignKey(
                        name: "FK_TagCurrentState_Reader_ReaderId",
                        column: x => x.ReaderId,
                        principalTable: "Reader",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TagCurrentState_Tag_EPC",
                        column: x => x.EPC,
                        principalTable: "Tag",
                        principalColumn: "EPC",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TagReadEvents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EPC = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ReaderId = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Antenna = table.Column<int>(type: "int", nullable: false),
                    Rssi = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TagReadEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TagReadEvents_Reader_ReaderId",
                        column: x => x.ReaderId,
                        principalTable: "Reader",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TagReadEvents_Tag_EPC",
                        column: x => x.EPC,
                        principalTable: "Tag",
                        principalColumn: "EPC",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tag_ProductId",
                table: "Tag",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_TagCurrentState_ReaderId",
                table: "TagCurrentState",
                column: "ReaderId");

            migrationBuilder.CreateIndex(
                name: "IX_TagReadEvents_EPC",
                table: "TagReadEvents",
                column: "EPC");

            migrationBuilder.CreateIndex(
                name: "IX_TagReadEvents_ReaderId",
                table: "TagReadEvents",
                column: "ReaderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TagCurrentState");

            migrationBuilder.DropTable(
                name: "TagReadEvents");

            migrationBuilder.DropTable(
                name: "Reader");

            migrationBuilder.DropTable(
                name: "Tag");

            migrationBuilder.DropTable(
                name: "Product");
        }
    }
}
