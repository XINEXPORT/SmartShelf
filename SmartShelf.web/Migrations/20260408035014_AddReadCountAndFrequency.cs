using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartShelf.web.Migrations
{
    /// <inheritdoc />
    public partial class AddReadCountAndFrequency : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TagReadEvents_Reader_ReaderId",
                table: "TagReadEvents");

            migrationBuilder.DropForeignKey(
                name: "FK_TagReadEvents_Tag_EPC",
                table: "TagReadEvents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TagReadEvents",
                table: "TagReadEvents");

            migrationBuilder.RenameTable(
                name: "TagReadEvents",
                newName: "TagReadEvent");

            migrationBuilder.RenameIndex(
                name: "IX_TagReadEvents_ReaderId",
                table: "TagReadEvent",
                newName: "IX_TagReadEvent_ReaderId");

            migrationBuilder.RenameIndex(
                name: "IX_TagReadEvents_EPC",
                table: "TagReadEvent",
                newName: "IX_TagReadEvent_EPC");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TagReadEvent",
                table: "TagReadEvent",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TagReadEvent_Reader_ReaderId",
                table: "TagReadEvent",
                column: "ReaderId",
                principalTable: "Reader",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TagReadEvent_Tag_EPC",
                table: "TagReadEvent",
                column: "EPC",
                principalTable: "Tag",
                principalColumn: "EPC",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TagReadEvent_Reader_ReaderId",
                table: "TagReadEvent");

            migrationBuilder.DropForeignKey(
                name: "FK_TagReadEvent_Tag_EPC",
                table: "TagReadEvent");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TagReadEvent",
                table: "TagReadEvent");

            migrationBuilder.RenameTable(
                name: "TagReadEvent",
                newName: "TagReadEvents");

            migrationBuilder.RenameIndex(
                name: "IX_TagReadEvent_ReaderId",
                table: "TagReadEvents",
                newName: "IX_TagReadEvents_ReaderId");

            migrationBuilder.RenameIndex(
                name: "IX_TagReadEvent_EPC",
                table: "TagReadEvents",
                newName: "IX_TagReadEvents_EPC");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TagReadEvents",
                table: "TagReadEvents",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TagReadEvents_Reader_ReaderId",
                table: "TagReadEvents",
                column: "ReaderId",
                principalTable: "Reader",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TagReadEvents_Tag_EPC",
                table: "TagReadEvents",
                column: "EPC",
                principalTable: "Tag",
                principalColumn: "EPC",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
