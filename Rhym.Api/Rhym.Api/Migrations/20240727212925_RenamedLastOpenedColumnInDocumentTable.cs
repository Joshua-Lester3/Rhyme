using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rhym.Api.Migrations
{
    /// <inheritdoc />
    public partial class RenamedLastOpenedColumnInDocumentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastOpened",
                table: "Documents",
                newName: "LastSaved");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastSaved",
                table: "Documents",
                newName: "LastOpened");
        }
    }
}
