using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DuAnBanBanhKeo.Migrations
{
    /// <inheritdoc />
    public partial class DLSS : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KhachHangs",
                columns: table => new
                {
                    MaKH = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TenKH = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SoDienThoai = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DiaChi = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KhachHangs", x => x.MaKH);
                });

            migrationBuilder.CreateTable(
                name: "NhaCungCaps",
                columns: table => new
                {
                    MaNCC = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TenNCC = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiaChi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SoDienThoai = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NhaCungCaps", x => x.MaNCC);
                });

            migrationBuilder.CreateTable(
                name: "NhanViens",
                columns: table => new
                {
                    MaNV = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    HoTen = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VaiTro = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SoDienThoai = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrangThai = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NhanViens", x => x.MaNV);
                });

            migrationBuilder.CreateTable(
                name: "PhieuNhaps",
                columns: table => new
                {
                    MaPhieuNhap = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NgayNhap = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TongTien = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaNCC = table.Column<int>(type: "int", nullable: true),
                    NhaCungCapMaNCC = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhieuNhaps", x => x.MaPhieuNhap);
                    table.ForeignKey(
                        name: "FK_PhieuNhaps_NhaCungCaps_NhaCungCapMaNCC",
                        column: x => x.NhaCungCapMaNCC,
                        principalTable: "NhaCungCaps",
                        principalColumn: "MaNCC");
                });

            migrationBuilder.CreateTable(
                name: "SanPhams",
                columns: table => new
                {
                    MaSP = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TenSP = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GiaNhap = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GiaBan = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SoLuongTon = table.Column<int>(type: "int", nullable: false),
                    DonViTinh = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrangThai = table.Column<bool>(type: "bit", nullable: false),
                    HinhAnh = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaNCC = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SanPhams", x => x.MaSP);
                    table.ForeignKey(
                        name: "FK_SanPhams_NhaCungCaps_MaNCC",
                        column: x => x.MaNCC,
                        principalTable: "NhaCungCaps",
                        principalColumn: "MaNCC");
                });

            migrationBuilder.CreateTable(
                name: "HoaDonNhaps",
                columns: table => new
                {
                    MaHoaDonNhap = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NgayNhap = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MaNV = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaNCC = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TongTien = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HoaDonNhaps", x => x.MaHoaDonNhap);
                    table.ForeignKey(
                        name: "FK_HoaDonNhaps_NhaCungCaps_MaNCC",
                        column: x => x.MaNCC,
                        principalTable: "NhaCungCaps",
                        principalColumn: "MaNCC",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HoaDonNhaps_NhanViens_MaNV",
                        column: x => x.MaNV,
                        principalTable: "NhanViens",
                        principalColumn: "MaNV",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HoaDonXuats",
                columns: table => new
                {
                    MaHoaDonXuat = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NgayXuat = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MaNV = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaKH = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TongTien = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HoaDonXuats", x => x.MaHoaDonXuat);
                    table.ForeignKey(
                        name: "FK_HoaDonXuats_KhachHangs_MaKH",
                        column: x => x.MaKH,
                        principalTable: "KhachHangs",
                        principalColumn: "MaKH",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HoaDonXuats_NhanViens_MaNV",
                        column: x => x.MaNV,
                        principalTable: "NhanViens",
                        principalColumn: "MaNV",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KiemKes",
                columns: table => new
                {
                    MaKiemKe = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NgayKiemKe = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MaNV = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    GhiChu = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KiemKes", x => x.MaKiemKe);
                    table.ForeignKey(
                        name: "FK_KiemKes_NhanViens_MaNV",
                        column: x => x.MaNV,
                        principalTable: "NhanViens",
                        principalColumn: "MaNV",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaiKhoans",
                columns: table => new
                {
                    MaTK = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TenDangNhap = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MatKhau = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaNV = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TrangThai = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaiKhoans", x => x.MaTK);
                    table.ForeignKey(
                        name: "FK_TaiKhoans_NhanViens_MaNV",
                        column: x => x.MaNV,
                        principalTable: "NhanViens",
                        principalColumn: "MaNV",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChiTietPhieuNhaps",
                columns: table => new
                {
                    MaChiTiet = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaPhieuNhap = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaSP = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    GiaNhap = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietPhieuNhaps", x => x.MaChiTiet);
                    table.ForeignKey(
                        name: "FK_ChiTietPhieuNhaps_PhieuNhaps_MaPhieuNhap",
                        column: x => x.MaPhieuNhap,
                        principalTable: "PhieuNhaps",
                        principalColumn: "MaPhieuNhap",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChiTietPhieuNhaps_SanPhams_MaSP",
                        column: x => x.MaSP,
                        principalTable: "SanPhams",
                        principalColumn: "MaSP",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChiTietHoaDonNhaps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaHoaDonNhap = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaSP = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    DonGia = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietHoaDonNhaps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChiTietHoaDonNhaps_HoaDonNhaps_MaHoaDonNhap",
                        column: x => x.MaHoaDonNhap,
                        principalTable: "HoaDonNhaps",
                        principalColumn: "MaHoaDonNhap",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChiTietHoaDonNhaps_SanPhams_MaSP",
                        column: x => x.MaSP,
                        principalTable: "SanPhams",
                        principalColumn: "MaSP",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChiTietHoaDonXuats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaHoaDonXuat = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaSP = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    DonGia = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietHoaDonXuats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChiTietHoaDonXuats_HoaDonXuats_MaHoaDonXuat",
                        column: x => x.MaHoaDonXuat,
                        principalTable: "HoaDonXuats",
                        principalColumn: "MaHoaDonXuat",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChiTietHoaDonXuats_SanPhams_MaSP",
                        column: x => x.MaSP,
                        principalTable: "SanPhams",
                        principalColumn: "MaSP",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChiTietKiemKes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaKiemKe = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaSP = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SoLuongThucTe = table.Column<int>(type: "int", nullable: false),
                    GhiChu = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietKiemKes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChiTietKiemKes_KiemKes_MaKiemKe",
                        column: x => x.MaKiemKe,
                        principalTable: "KiemKes",
                        principalColumn: "MaKiemKe",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChiTietKiemKes_SanPhams_MaSP",
                        column: x => x.MaSP,
                        principalTable: "SanPhams",
                        principalColumn: "MaSP",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "KhachHangs",
                columns: new[] { "MaKH", "DiaChi", "Email", "SoDienThoai", "TenKH" },
                values: new object[,]
                {
                    { "KH001", "Hà Nội", null, "0987654321", "Nguyễn An" },
                    { "KH002", "Đà Nẵng", null, "0976543210", "Lê Bình" },
                    { "KH003", "TP. HCM", null, "0965432109", "Trần Cường" }
                });

            migrationBuilder.InsertData(
                table: "NhaCungCaps",
                columns: new[] { "MaNCC", "DiaChi", "Email", "SoDienThoai", "TenNCC" },
                values: new object[,]
                {
                    { "NCC001", "Hà Nội", null, "0901123456", "Coca-Cola VN" },
                    { "NCC002", "TP. Hồ Chí Minh", null, "0902234567", "PepsiCo VN" },
                    { "NCC003", "Đà Nẵng", null, "0903345678", "Mondelez VN" }
                });

            migrationBuilder.InsertData(
                table: "NhanViens",
                columns: new[] { "MaNV", "Email", "HoTen", "SoDienThoai", "TrangThai", "VaiTro" },
                values: new object[,]
                {
                    { "NV001", "a.nguyen@example.com", "Nguyễn Văn An", "0911123456", true, "Nhân viên" },
                    { "NV002", "b.tran@example.com", "Trần Thị Bông", "0912234567", true, "Quản lý kho" },
                    { "NV003", "c.le@example.com", "Lê Văn Thuận", "0913345678", false, "Nhân viên" }
                });

            migrationBuilder.InsertData(
                table: "HoaDonNhaps",
                columns: new[] { "MaHoaDonNhap", "MaNCC", "MaNV", "NgayNhap", "TongTien" },
                values: new object[,]
                {
                    { "HDN001", "NCC001", "NV001", new DateTime(2024, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2000000m },
                    { "HDN002", "NCC002", "NV001", new DateTime(2024, 6, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 3500000m }
                });

            migrationBuilder.InsertData(
                table: "HoaDonXuats",
                columns: new[] { "MaHoaDonXuat", "MaKH", "MaNV", "NgayXuat", "TongTien" },
                values: new object[,]
                {
                    { "HDX001", "KH001", "NV002", new DateTime(2024, 6, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 500000m },
                    { "HDX002", "KH002", "NV002", new DateTime(2024, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 750000m }
                });

            migrationBuilder.InsertData(
                table: "KiemKes",
                columns: new[] { "MaKiemKe", "GhiChu", "MaNV", "NgayKiemKe" },
                values: new object[,]
                {
                    { "KK001", "Kiểm kê định kỳ", "NV003", new DateTime(2024, 6, 20, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { "KK002", "Sai lệch số lượng", "NV003", new DateTime(2024, 6, 30, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "SanPhams",
                columns: new[] { "MaSP", "DonViTinh", "GiaBan", "GiaNhap", "HinhAnh", "MaNCC", "SoLuongTon", "TenSP", "TrangThai" },
                values: new object[,]
                {
                    { "SP001", "Lon", 10000m, 5000m, null, "NCC001", 100, "Coca-Cola 330ml", true },
                    { "SP002", "Lon", 9500m, 4800m, null, "NCC002", 120, "Pepsi 330ml", true },
                    { "SP003", "Gói", 15000m, 7000m, null, "NCC003", 50, "Bánh Oreo", true },
                    { "SP004", "Gói", 18000m, 9000m, null, "NCC001", 75, "Snack Lay's", true }
                });

            migrationBuilder.InsertData(
                table: "TaiKhoans",
                columns: new[] { "MaTK", "MaNV", "MatKhau", "TenDangNhap", "TrangThai" },
                values: new object[,]
                {
                    { "TK001", "NV001", "123", "user", true },
                    { "TK002", "NV002", "123", "admin", true }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietHoaDonNhaps_MaHoaDonNhap",
                table: "ChiTietHoaDonNhaps",
                column: "MaHoaDonNhap");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietHoaDonNhaps_MaSP",
                table: "ChiTietHoaDonNhaps",
                column: "MaSP");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietHoaDonXuats_MaHoaDonXuat",
                table: "ChiTietHoaDonXuats",
                column: "MaHoaDonXuat");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietHoaDonXuats_MaSP",
                table: "ChiTietHoaDonXuats",
                column: "MaSP");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietKiemKes_MaKiemKe",
                table: "ChiTietKiemKes",
                column: "MaKiemKe");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietKiemKes_MaSP",
                table: "ChiTietKiemKes",
                column: "MaSP");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietPhieuNhaps_MaPhieuNhap",
                table: "ChiTietPhieuNhaps",
                column: "MaPhieuNhap");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietPhieuNhaps_MaSP",
                table: "ChiTietPhieuNhaps",
                column: "MaSP");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonNhaps_MaNCC",
                table: "HoaDonNhaps",
                column: "MaNCC");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonNhaps_MaNV",
                table: "HoaDonNhaps",
                column: "MaNV");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonXuats_MaKH",
                table: "HoaDonXuats",
                column: "MaKH");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonXuats_MaNV",
                table: "HoaDonXuats",
                column: "MaNV");

            migrationBuilder.CreateIndex(
                name: "IX_KiemKes_MaNV",
                table: "KiemKes",
                column: "MaNV");

            migrationBuilder.CreateIndex(
                name: "IX_PhieuNhaps_NhaCungCapMaNCC",
                table: "PhieuNhaps",
                column: "NhaCungCapMaNCC");

            migrationBuilder.CreateIndex(
                name: "IX_SanPhams_MaNCC",
                table: "SanPhams",
                column: "MaNCC");

            migrationBuilder.CreateIndex(
                name: "IX_TaiKhoans_MaNV",
                table: "TaiKhoans",
                column: "MaNV",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChiTietHoaDonNhaps");

            migrationBuilder.DropTable(
                name: "ChiTietHoaDonXuats");

            migrationBuilder.DropTable(
                name: "ChiTietKiemKes");

            migrationBuilder.DropTable(
                name: "ChiTietPhieuNhaps");

            migrationBuilder.DropTable(
                name: "TaiKhoans");

            migrationBuilder.DropTable(
                name: "HoaDonNhaps");

            migrationBuilder.DropTable(
                name: "HoaDonXuats");

            migrationBuilder.DropTable(
                name: "KiemKes");

            migrationBuilder.DropTable(
                name: "PhieuNhaps");

            migrationBuilder.DropTable(
                name: "SanPhams");

            migrationBuilder.DropTable(
                name: "KhachHangs");

            migrationBuilder.DropTable(
                name: "NhanViens");

            migrationBuilder.DropTable(
                name: "NhaCungCaps");
        }
    }
}
