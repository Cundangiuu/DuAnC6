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
                name: "Combos",
                columns: table => new
                {
                    MaCombo = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TenCombo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gia = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TrangThai = table.Column<int>(type: "int", nullable: false),
                    Anh = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SoLuongCombo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Combos", x => x.MaCombo);
                });

            migrationBuilder.CreateTable(
                name: "GiamGias",
                columns: table => new
                {
                    MaGiamGia = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TenGiamGia = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GiaTri = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NgayBatDau = table.Column<DateOnly>(type: "date", nullable: false),
                    NgayKetThuc = table.Column<DateOnly>(type: "date", nullable: false),
                    SoLuongMaNhapToiDa = table.Column<int>(type: "int", nullable: false),
                    TrangThai = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GiamGias", x => x.MaGiamGia);
                });

            migrationBuilder.CreateTable(
                name: "KhachHangs",
                columns: table => new
                {
                    MaKhachHang = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TenKhachHang = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SoDienThoai = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DiaChi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NgayDangKy = table.Column<DateOnly>(type: "date", nullable: false),
                    HinhAnh = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cccd = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NgaySinh = table.Column<DateOnly>(type: "date", nullable: true),
                    GioiTinh = table.Column<bool>(type: "bit", nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KhachHangs", x => x.MaKhachHang);
                });

            migrationBuilder.CreateTable(
                name: "NhaCungCapBanhs",
                columns: table => new
                {
                    MaNhaCungCapBanh = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenNhaCungCap = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NhaCungCapBanhs", x => x.MaNhaCungCapBanh);
                });

            migrationBuilder.CreateTable(
                name: "NhaCungCapKeos",
                columns: table => new
                {
                    MaNhaCungCapKeo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenNhaCungCap = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NhaCungCapKeos", x => x.MaNhaCungCapKeo);
                });

            migrationBuilder.CreateTable(
                name: "NhaCungCapNuocNgots",
                columns: table => new
                {
                    MaNhaCungCapNuocNgot = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenNhaCungCap = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NhaCungCapNuocNgots", x => x.MaNhaCungCapNuocNgot);
                });

            migrationBuilder.CreateTable(
                name: "NhanViens",
                columns: table => new
                {
                    MaNhanVien = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TenNhanVien = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VaiTro = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SoDienThoai = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NgayBatDau = table.Column<DateOnly>(type: "date", nullable: true),
                    HinhAnh = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NgaySinh = table.Column<DateOnly>(type: "date", nullable: true),
                    DiaChi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: true),
                    Cccd = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GioiTinh = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NhanViens", x => x.MaNhanVien);
                });

            migrationBuilder.CreateTable(
                name: "MaNhapGiamGias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaNhap = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaGiamGia = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsUsed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaNhapGiamGias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaNhapGiamGias_GiamGias_MaGiamGia",
                        column: x => x.MaGiamGia,
                        principalTable: "GiamGias",
                        principalColumn: "MaGiamGia",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DonHangs",
                columns: table => new
                {
                    MaDonHang = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaKhachHang = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NgayDatHang = table.Column<DateOnly>(type: "date", nullable: false),
                    TongTien = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TrangThai = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DonHangs", x => x.MaDonHang);
                    table.ForeignKey(
                        name: "FK_DonHangs_KhachHangs_MaKhachHang",
                        column: x => x.MaKhachHang,
                        principalTable: "KhachHangs",
                        principalColumn: "MaKhachHang",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GioHangs",
                columns: table => new
                {
                    MaGioHang = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaKhachHang = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    NgayTao = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GioHangs", x => x.MaGioHang);
                    table.ForeignKey(
                        name: "FK_GioHangs_KhachHangs_MaKhachHang",
                        column: x => x.MaKhachHang,
                        principalTable: "KhachHangs",
                        principalColumn: "MaKhachHang");
                });

            migrationBuilder.CreateTable(
                name: "HoaDons",
                columns: table => new
                {
                    MaHoaDon = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaKhachHang = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    NgayXuatHoaDon = table.Column<DateOnly>(type: "date", nullable: false),
                    TongTien = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TrangThai = table.Column<int>(type: "int", nullable: true),
                    MaGiamGia = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HoaDons", x => x.MaHoaDon);
                    table.ForeignKey(
                        name: "FK_HoaDons_GiamGias_MaGiamGia",
                        column: x => x.MaGiamGia,
                        principalTable: "GiamGias",
                        principalColumn: "MaGiamGia",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_HoaDons_KhachHangs_MaKhachHang",
                        column: x => x.MaKhachHang,
                        principalTable: "KhachHangs",
                        principalColumn: "MaKhachHang");
                });

            migrationBuilder.CreateTable(
                name: "SanPhams",
                columns: table => new
                {
                    MaSanPham = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TenSanPham = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gia = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    NgayThem = table.Column<DateOnly>(type: "date", nullable: true),
                    DonVi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nsx = table.Column<DateOnly>(type: "date", nullable: true),
                    Hsd = table.Column<DateOnly>(type: "date", nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: true),
                    MaNhaCungCapBanh = table.Column<int>(type: "int", nullable: true),
                    MaNhaCungCapKeo = table.Column<int>(type: "int", nullable: true),
                    MaNhaCungCapNuocNgot = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SanPhams", x => x.MaSanPham);
                    table.ForeignKey(
                        name: "FK_SanPhams_NhaCungCapBanhs_MaNhaCungCapBanh",
                        column: x => x.MaNhaCungCapBanh,
                        principalTable: "NhaCungCapBanhs",
                        principalColumn: "MaNhaCungCapBanh");
                    table.ForeignKey(
                        name: "FK_SanPhams_NhaCungCapKeos_MaNhaCungCapKeo",
                        column: x => x.MaNhaCungCapKeo,
                        principalTable: "NhaCungCapKeos",
                        principalColumn: "MaNhaCungCapKeo");
                    table.ForeignKey(
                        name: "FK_SanPhams_NhaCungCapNuocNgots_MaNhaCungCapNuocNgot",
                        column: x => x.MaNhaCungCapNuocNgot,
                        principalTable: "NhaCungCapNuocNgots",
                        principalColumn: "MaNhaCungCapNuocNgot");
                });

            migrationBuilder.CreateTable(
                name: "TaiKhoans",
                columns: table => new
                {
                    MaTaiKhoan = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TenTaiKhoan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MatKhau = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VaiTro = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaKhachHang = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    MaNhanVien = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TinhTrang = table.Column<int>(type: "int", nullable: false),
                    MaNhanVienNavigationMaNhanVien = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaiKhoans", x => x.MaTaiKhoan);
                    table.ForeignKey(
                        name: "FK_TaiKhoans_KhachHangs_MaKhachHang",
                        column: x => x.MaKhachHang,
                        principalTable: "KhachHangs",
                        principalColumn: "MaKhachHang");
                    table.ForeignKey(
                        name: "FK_TaiKhoans_NhanViens_MaNhanVienNavigationMaNhanVien",
                        column: x => x.MaNhanVienNavigationMaNhanVien,
                        principalTable: "NhanViens",
                        principalColumn: "MaNhanVien");
                });

            migrationBuilder.CreateTable(
                name: "AnhSanPhams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaSanPham = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UrlAnh = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnhSanPhams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnhSanPhams_SanPhams_MaSanPham",
                        column: x => x.MaSanPham,
                        principalTable: "SanPhams",
                        principalColumn: "MaSanPham",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChiTietCombos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaCombo = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaSanPham = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SoLuongCombo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietCombos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChiTietCombos_Combos_MaCombo",
                        column: x => x.MaCombo,
                        principalTable: "Combos",
                        principalColumn: "MaCombo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChiTietCombos_SanPhams_MaSanPham",
                        column: x => x.MaSanPham,
                        principalTable: "SanPhams",
                        principalColumn: "MaSanPham",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChiTietDonHangs",
                columns: table => new
                {
                    MaChiTietDonHang = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaDonHang = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaSanPham = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    MaCombo = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    Gia = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietDonHangs", x => x.MaChiTietDonHang);
                    table.ForeignKey(
                        name: "FK_ChiTietDonHangs_Combos_MaCombo",
                        column: x => x.MaCombo,
                        principalTable: "Combos",
                        principalColumn: "MaCombo");
                    table.ForeignKey(
                        name: "FK_ChiTietDonHangs_DonHangs_MaDonHang",
                        column: x => x.MaDonHang,
                        principalTable: "DonHangs",
                        principalColumn: "MaDonHang",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChiTietDonHangs_SanPhams_MaSanPham",
                        column: x => x.MaSanPham,
                        principalTable: "SanPhams",
                        principalColumn: "MaSanPham");
                });

            migrationBuilder.CreateTable(
                name: "ChiTietGioHangs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaGioHang = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaSanPham = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    MaCombo = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    SoLuong = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietGioHangs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChiTietGioHangs_Combos_MaCombo",
                        column: x => x.MaCombo,
                        principalTable: "Combos",
                        principalColumn: "MaCombo");
                    table.ForeignKey(
                        name: "FK_ChiTietGioHangs_GioHangs_MaGioHang",
                        column: x => x.MaGioHang,
                        principalTable: "GioHangs",
                        principalColumn: "MaGioHang",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChiTietGioHangs_SanPhams_MaSanPham",
                        column: x => x.MaSanPham,
                        principalTable: "SanPhams",
                        principalColumn: "MaSanPham");
                });

            migrationBuilder.CreateTable(
                name: "ChiTietHoaDons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaHoaDon = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaSanPham = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    MaCombo = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    Gia = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietHoaDons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChiTietHoaDons_Combos_MaCombo",
                        column: x => x.MaCombo,
                        principalTable: "Combos",
                        principalColumn: "MaCombo");
                    table.ForeignKey(
                        name: "FK_ChiTietHoaDons_HoaDons_MaHoaDon",
                        column: x => x.MaHoaDon,
                        principalTable: "HoaDons",
                        principalColumn: "MaHoaDon",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChiTietHoaDons_SanPhams_MaSanPham",
                        column: x => x.MaSanPham,
                        principalTable: "SanPhams",
                        principalColumn: "MaSanPham");
                });

            migrationBuilder.InsertData(
                table: "Combos",
                columns: new[] { "MaCombo", "Anh", "Gia", "MoTa", "SoLuongCombo", "TenCombo", "TrangThai" },
                values: new object[,]
                {
                    { "C001", "combo-banh-keo.jpg", 200000m, "Combo gồm 1 bánh kem sôcôla và 2 hộp kẹo dẻo", 50, "Combo Bánh & Kẹo", 1 },
                    { "C002", "combo-nuoc-ngot.jpg", 180000m, "Combo gồm 1 bánh kem sôcôla và 1 nước ngọt Cola", 80, "Combo Nước Ngọt & Bánh", 1 }
                });

            migrationBuilder.InsertData(
                table: "SanPhams",
                columns: new[] { "MaSanPham", "DonVi", "Gia", "Hsd", "MaNhaCungCapBanh", "MaNhaCungCapKeo", "MaNhaCungCapNuocNgot", "MoTa", "NgayThem", "Nsx", "SoLuong", "TenSanPham", "TrangThai" },
                values: new object[,]
                {
                    { "SP001", "Cái", 150000m, new DateOnly(2025, 9, 11), null, null, null, "Bánh kem sôcôla tươi ngon", new DateOnly(2025, 3, 11), new DateOnly(2024, 12, 11), 100, "Bánh Kem Sôcôla", 1 },
                    { "SP002", "Hộp", 50000m, new DateOnly(2026, 3, 11), null, null, null, "Kẹo dẻo ngon miệng cho mọi lứa tuổi", new DateOnly(2025, 3, 11), new DateOnly(2025, 1, 11), 200, "Kẹo Dẻo", 1 },
                    { "SP003", "Lít", 10000m, new DateOnly(2025, 11, 11), null, null, null, "Nước ngọt cola cho mùa hè mát lạnh", new DateOnly(2025, 3, 11), new DateOnly(2025, 2, 11), 300, "Nước Ngọt Cola", 1 }
                });

            migrationBuilder.InsertData(
                table: "ChiTietCombos",
                columns: new[] { "Id", "MaCombo", "MaSanPham", "SoLuongCombo" },
                values: new object[,]
                {
                    { 1, "C001", "SP001", 0 },
                    { 2, "C001", "SP002", 0 },
                    { 3, "C002", "SP001", 0 },
                    { 4, "C002", "SP002", 0 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnhSanPhams_MaSanPham",
                table: "AnhSanPhams",
                column: "MaSanPham");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietCombos_MaCombo",
                table: "ChiTietCombos",
                column: "MaCombo");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietCombos_MaSanPham",
                table: "ChiTietCombos",
                column: "MaSanPham");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietDonHangs_MaCombo",
                table: "ChiTietDonHangs",
                column: "MaCombo");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietDonHangs_MaDonHang",
                table: "ChiTietDonHangs",
                column: "MaDonHang");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietDonHangs_MaSanPham",
                table: "ChiTietDonHangs",
                column: "MaSanPham");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietGioHangs_MaCombo",
                table: "ChiTietGioHangs",
                column: "MaCombo");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietGioHangs_MaGioHang",
                table: "ChiTietGioHangs",
                column: "MaGioHang");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietGioHangs_MaSanPham",
                table: "ChiTietGioHangs",
                column: "MaSanPham");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietHoaDons_MaCombo",
                table: "ChiTietHoaDons",
                column: "MaCombo");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietHoaDons_MaHoaDon",
                table: "ChiTietHoaDons",
                column: "MaHoaDon");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietHoaDons_MaSanPham",
                table: "ChiTietHoaDons",
                column: "MaSanPham");

            migrationBuilder.CreateIndex(
                name: "IX_DonHangs_MaKhachHang",
                table: "DonHangs",
                column: "MaKhachHang");

            migrationBuilder.CreateIndex(
                name: "IX_GioHangs_MaKhachHang",
                table: "GioHangs",
                column: "MaKhachHang");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDons_MaGiamGia",
                table: "HoaDons",
                column: "MaGiamGia");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDons_MaKhachHang",
                table: "HoaDons",
                column: "MaKhachHang");

            migrationBuilder.CreateIndex(
                name: "IX_MaNhapGiamGias_MaGiamGia",
                table: "MaNhapGiamGias",
                column: "MaGiamGia");

            migrationBuilder.CreateIndex(
                name: "IX_SanPhams_MaNhaCungCapBanh",
                table: "SanPhams",
                column: "MaNhaCungCapBanh");

            migrationBuilder.CreateIndex(
                name: "IX_SanPhams_MaNhaCungCapKeo",
                table: "SanPhams",
                column: "MaNhaCungCapKeo");

            migrationBuilder.CreateIndex(
                name: "IX_SanPhams_MaNhaCungCapNuocNgot",
                table: "SanPhams",
                column: "MaNhaCungCapNuocNgot");

            migrationBuilder.CreateIndex(
                name: "IX_TaiKhoans_MaKhachHang",
                table: "TaiKhoans",
                column: "MaKhachHang");

            migrationBuilder.CreateIndex(
                name: "IX_TaiKhoans_MaNhanVienNavigationMaNhanVien",
                table: "TaiKhoans",
                column: "MaNhanVienNavigationMaNhanVien");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnhSanPhams");

            migrationBuilder.DropTable(
                name: "ChiTietCombos");

            migrationBuilder.DropTable(
                name: "ChiTietDonHangs");

            migrationBuilder.DropTable(
                name: "ChiTietGioHangs");

            migrationBuilder.DropTable(
                name: "ChiTietHoaDons");

            migrationBuilder.DropTable(
                name: "MaNhapGiamGias");

            migrationBuilder.DropTable(
                name: "TaiKhoans");

            migrationBuilder.DropTable(
                name: "DonHangs");

            migrationBuilder.DropTable(
                name: "GioHangs");

            migrationBuilder.DropTable(
                name: "Combos");

            migrationBuilder.DropTable(
                name: "HoaDons");

            migrationBuilder.DropTable(
                name: "SanPhams");

            migrationBuilder.DropTable(
                name: "NhanViens");

            migrationBuilder.DropTable(
                name: "GiamGias");

            migrationBuilder.DropTable(
                name: "KhachHangs");

            migrationBuilder.DropTable(
                name: "NhaCungCapBanhs");

            migrationBuilder.DropTable(
                name: "NhaCungCapKeos");

            migrationBuilder.DropTable(
                name: "NhaCungCapNuocNgots");
        }
    }
}
