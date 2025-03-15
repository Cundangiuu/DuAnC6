using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DuAnBanBanhKeo.Migrations
{
    /// <inheritdoc />
    public partial class fsgfs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HinhAnh",
                table: "NhanViens",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "NhanViens",
                keyColumn: "MaNV",
                keyValue: "NV001",
                column: "HinhAnh",
                value: null);

            migrationBuilder.UpdateData(
                table: "NhanViens",
                keyColumn: "MaNV",
                keyValue: "NV002",
                column: "HinhAnh",
                value: null);

            migrationBuilder.UpdateData(
                table: "NhanViens",
                keyColumn: "MaNV",
                keyValue: "NV003",
                column: "HinhAnh",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HinhAnh",
                table: "NhanViens");
        }
    }
}
