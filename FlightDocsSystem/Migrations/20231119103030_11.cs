using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlightDocsSystem.Migrations
{
    /// <inheritdoc />
    public partial class _11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "user",
                columns: new[] { "Id", "email", "password_hash", "password_salt", "phone", "role", "user_name" },
                values: new object[] { 1, "admin@example.com", new byte[] { 167, 250, 207, 17, 127, 199, 147, 83, 183, 231, 237, 136, 177, 170, 14, 216, 40, 54, 186, 0, 43, 133, 2, 139, 110, 205, 115, 4, 78, 145, 252, 160, 109, 200, 73, 156, 5, 245, 34, 85, 251, 236, 223, 235, 70, 158, 167, 5, 144, 133, 100, 112, 140, 126, 204, 91, 224, 232, 222, 17, 0, 163, 148, 88 }, new byte[] { 136, 134, 244, 135, 221, 247, 160, 53, 196, 32, 105, 86, 202, 156, 142, 239, 19, 107, 156, 206, 116, 11, 169, 86, 34, 66, 186, 243, 80, 190, 16, 128, 204, 58, 4, 47, 106, 93, 81, 215, 164, 42, 193, 24, 36, 188, 154, 147, 240, 33, 204, 48, 88, 235, 40, 142, 242, 87, 116, 8, 67, 241, 23, 8, 116, 90, 17, 202, 120, 158, 57, 179, 48, 166, 230, 125, 103, 62, 20, 149, 174, 236, 50, 227, 222, 100, 93, 113, 229, 81, 93, 90, 179, 71, 196, 4, 206, 21, 171, 54, 233, 239, 184, 71, 104, 60, 92, 168, 49, 247, 40, 74, 145, 162, 31, 199, 152, 173, 60, 169, 88, 184, 120, 214, 78, 68, 121, 150 }, "1234567890", "Admin", "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "user",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
