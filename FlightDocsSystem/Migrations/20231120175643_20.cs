using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlightDocsSystem.Migrations
{
    /// <inheritdoc />
    public partial class _20 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "role",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "role_name", "role_normalized_name" },
                values: new object[] { "Employee", "EMPLOYEE" });

            migrationBuilder.UpdateData(
                table: "role",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "role_name", "role_normalized_name" },
                values: new object[] { "Pilot", "PILOT" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "role",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "role_name", "role_normalized_name" },
                values: new object[] { "Pilot", "PILOT" });

            migrationBuilder.UpdateData(
                table: "role",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "role_name", "role_normalized_name" },
                values: new object[] { "Employee", "EMPLOYEE" });
        }
    }
}
