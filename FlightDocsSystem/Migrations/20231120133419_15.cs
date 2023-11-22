using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlightDocsSystem.Migrations
{
    /// <inheritdoc />
    public partial class _15 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_doc",
                table: "doc");

            migrationBuilder.RenameTable(
                name: "doc",
                newName: "document");

            migrationBuilder.AddPrimaryKey(
                name: "PK_document",
                table: "document",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_document",
                table: "document");

            migrationBuilder.RenameTable(
                name: "document",
                newName: "doc");

            migrationBuilder.AddPrimaryKey(
                name: "PK_doc",
                table: "doc",
                column: "Id");
        }
    }
}
