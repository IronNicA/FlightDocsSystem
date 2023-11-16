using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlightDocsSystem.Migrations
{
    /// <inheritdoc />
    public partial class _08 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_user_AspNetRoles_RoleId",
                table: "user");

            migrationBuilder.DropIndex(
                name: "IX_user_RoleId",
                table: "user");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "user");

            migrationBuilder.AddColumn<string>(
                name: "role",
                table: "user",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "role",
                table: "user");

            migrationBuilder.AddColumn<string>(
                name: "RoleId",
                table: "user",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_RoleId",
                table: "user",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_user_AspNetRoles_RoleId",
                table: "user",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id");
        }
    }
}
