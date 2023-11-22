using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlightDocsSystem.Migrations
{
    /// <inheritdoc />
    public partial class _13 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RolePermissions_DocTypes_DocTypeId",
                table: "RolePermissions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RolePermissions",
                table: "RolePermissions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DocTypes",
                table: "DocTypes");

            migrationBuilder.RenameTable(
                name: "RolePermissions",
                newName: "role_permission");

            migrationBuilder.RenameTable(
                name: "DocTypes",
                newName: "doc_type");

            migrationBuilder.RenameIndex(
                name: "IX_RolePermissions_DocTypeId",
                table: "role_permission",
                newName: "IX_role_permission_DocTypeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_role_permission",
                table: "role_permission",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_doc_type",
                table: "doc_type",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_role_permission_doc_type_DocTypeId",
                table: "role_permission",
                column: "DocTypeId",
                principalTable: "doc_type",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_role_permission_doc_type_DocTypeId",
                table: "role_permission");

            migrationBuilder.DropPrimaryKey(
                name: "PK_role_permission",
                table: "role_permission");

            migrationBuilder.DropPrimaryKey(
                name: "PK_doc_type",
                table: "doc_type");

            migrationBuilder.RenameTable(
                name: "role_permission",
                newName: "RolePermissions");

            migrationBuilder.RenameTable(
                name: "doc_type",
                newName: "DocTypes");

            migrationBuilder.RenameIndex(
                name: "IX_role_permission_DocTypeId",
                table: "RolePermissions",
                newName: "IX_RolePermissions_DocTypeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RolePermissions",
                table: "RolePermissions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DocTypes",
                table: "DocTypes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RolePermissions_DocTypes_DocTypeId",
                table: "RolePermissions",
                column: "DocTypeId",
                principalTable: "DocTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
