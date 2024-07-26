using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rhym.Api.Migrations
{
    /// <inheritdoc />
    public partial class RefactorDocumentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_DocumentData_DocumentDataId",
                table: "Documents");

            migrationBuilder.DropTable(
                name: "DocumentData");

            migrationBuilder.DropIndex(
                name: "IX_Documents_DocumentDataId",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "DocumentDataId",
                table: "Documents");

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "Documents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Content",
                table: "Documents");

            migrationBuilder.AddColumn<int>(
                name: "DocumentDataId",
                table: "Documents",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "DocumentData",
                columns: table => new
                {
                    DocumentDataId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentData", x => x.DocumentDataId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Documents_DocumentDataId",
                table: "Documents",
                column: "DocumentDataId");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_DocumentData_DocumentDataId",
                table: "Documents",
                column: "DocumentDataId",
                principalTable: "DocumentData",
                principalColumn: "DocumentDataId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
