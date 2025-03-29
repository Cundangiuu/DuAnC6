using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DuAnBanBanhKeo.Migrations
{
    /// <inheritdoc />
    public partial class AddEmailToTaiKhoan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "TaiKhoans",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "TaiKhoans",
                keyColumn: "MaTK",
                keyValue: "TK001",
                column: "Email",
                value: "user@example.com");

            migrationBuilder.UpdateData(
                table: "TaiKhoans",
                keyColumn: "MaTK",
                keyValue: "TK002",
                column: "Email",
                value: "admin@example.com");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "TaiKhoans");
        }
    }
}
