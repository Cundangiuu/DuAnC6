﻿// <auto-generated />
using System;
using DuAnBanBanhKeo.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DuAnBanBanhKeo.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250314124836_fsgfs")]
    partial class fsgfs
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DuAnBanBanhKeo.Data.Entities.ChiTietHoaDonNhap", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("DonGia")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("MaHoaDonNhap")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("MaSP")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("SoLuong")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("MaHoaDonNhap");

                    b.HasIndex("MaSP");

                    b.ToTable("ChiTietHoaDonNhaps");
                });

            modelBuilder.Entity("DuAnBanBanhKeo.Data.Entities.ChiTietHoaDonXuat", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("DonGia")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("MaHoaDonXuat")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("MaSP")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("SoLuong")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("MaHoaDonXuat");

                    b.HasIndex("MaSP");

                    b.ToTable("ChiTietHoaDonXuats");
                });

            modelBuilder.Entity("DuAnBanBanhKeo.Data.Entities.ChiTietKiemKe", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("GhiChu")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MaKiemKe")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("MaSP")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("SoLuongThucTe")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("MaKiemKe");

                    b.HasIndex("MaSP");

                    b.ToTable("ChiTietKiemKes");
                });

            modelBuilder.Entity("DuAnBanBanhKeo.Data.Entities.ChiTietPhieuNhap", b =>
                {
                    b.Property<int>("MaChiTiet")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MaChiTiet"));

                    b.Property<decimal>("GiaNhap")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("MaPhieuNhap")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("MaSP")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("SoLuong")
                        .HasColumnType("int");

                    b.HasKey("MaChiTiet");

                    b.HasIndex("MaPhieuNhap");

                    b.HasIndex("MaSP");

                    b.ToTable("ChiTietPhieuNhaps");
                });

            modelBuilder.Entity("DuAnBanBanhKeo.Data.Entities.HoaDonNhap", b =>
                {
                    b.Property<string>("MaHoaDonNhap")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("MaNCC")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("MaNV")
                        .IsRequired()
                        .HasColumnType("nvarchar(10)");

                    b.Property<DateTime>("NgayNhap")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("TongTien")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("MaHoaDonNhap");

                    b.HasIndex("MaNCC");

                    b.HasIndex("MaNV");

                    b.ToTable("HoaDonNhaps");

                    b.HasData(
                        new
                        {
                            MaHoaDonNhap = "HDN001",
                            MaNCC = "NCC001",
                            MaNV = "NV001",
                            NgayNhap = new DateTime(2024, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            TongTien = 2000000m
                        },
                        new
                        {
                            MaHoaDonNhap = "HDN002",
                            MaNCC = "NCC002",
                            MaNV = "NV001",
                            NgayNhap = new DateTime(2024, 6, 5, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            TongTien = 3500000m
                        });
                });

            modelBuilder.Entity("DuAnBanBanhKeo.Data.Entities.HoaDonXuat", b =>
                {
                    b.Property<string>("MaHoaDonXuat")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("MaKH")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("MaNV")
                        .IsRequired()
                        .HasColumnType("nvarchar(10)");

                    b.Property<DateTime>("NgayXuat")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("TongTien")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("MaHoaDonXuat");

                    b.HasIndex("MaKH");

                    b.HasIndex("MaNV");

                    b.ToTable("HoaDonXuats");

                    b.HasData(
                        new
                        {
                            MaHoaDonXuat = "HDX001",
                            MaKH = "KH001",
                            MaNV = "NV002",
                            NgayXuat = new DateTime(2024, 6, 10, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            TongTien = 500000m
                        },
                        new
                        {
                            MaHoaDonXuat = "HDX002",
                            MaKH = "KH002",
                            MaNV = "NV002",
                            NgayXuat = new DateTime(2024, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            TongTien = 750000m
                        });
                });

            modelBuilder.Entity("DuAnBanBanhKeo.Data.Entities.KhachHang", b =>
                {
                    b.Property<string>("MaKH")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("DiaChi")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SoDienThoai")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TenKH")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MaKH");

                    b.ToTable("KhachHangs");

                    b.HasData(
                        new
                        {
                            MaKH = "KH001",
                            DiaChi = "Hà Nội",
                            SoDienThoai = "0987654321",
                            TenKH = "Nguyễn An"
                        },
                        new
                        {
                            MaKH = "KH002",
                            DiaChi = "Đà Nẵng",
                            SoDienThoai = "0976543210",
                            TenKH = "Lê Bình"
                        },
                        new
                        {
                            MaKH = "KH003",
                            DiaChi = "TP. HCM",
                            SoDienThoai = "0965432109",
                            TenKH = "Trần Cường"
                        });
                });

            modelBuilder.Entity("DuAnBanBanhKeo.Data.Entities.KiemKe", b =>
                {
                    b.Property<string>("MaKiemKe")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("GhiChu")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MaNV")
                        .IsRequired()
                        .HasColumnType("nvarchar(10)");

                    b.Property<DateTime>("NgayKiemKe")
                        .HasColumnType("datetime2");

                    b.HasKey("MaKiemKe");

                    b.HasIndex("MaNV");

                    b.ToTable("KiemKes");

                    b.HasData(
                        new
                        {
                            MaKiemKe = "KK001",
                            GhiChu = "Kiểm kê định kỳ",
                            MaNV = "NV003",
                            NgayKiemKe = new DateTime(2024, 6, 20, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            MaKiemKe = "KK002",
                            GhiChu = "Sai lệch số lượng",
                            MaNV = "NV003",
                            NgayKiemKe = new DateTime(2024, 6, 30, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        });
                });

            modelBuilder.Entity("DuAnBanBanhKeo.Data.Entities.NhaCungCap", b =>
                {
                    b.Property<string>("MaNCC")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("DiaChi")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SoDienThoai")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TenNCC")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MaNCC");

                    b.ToTable("NhaCungCaps");

                    b.HasData(
                        new
                        {
                            MaNCC = "NCC001",
                            DiaChi = "Hà Nội",
                            SoDienThoai = "0901123456",
                            TenNCC = "Coca-Cola VN"
                        },
                        new
                        {
                            MaNCC = "NCC002",
                            DiaChi = "TP. Hồ Chí Minh",
                            SoDienThoai = "0902234567",
                            TenNCC = "PepsiCo VN"
                        },
                        new
                        {
                            MaNCC = "NCC003",
                            DiaChi = "Đà Nẵng",
                            SoDienThoai = "0903345678",
                            TenNCC = "Mondelez VN"
                        });
                });

            modelBuilder.Entity("DuAnBanBanhKeo.Data.Entities.NhanVien", b =>
                {
                    b.Property<string>("MaNV")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HinhAnh")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HoTen")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("SoDienThoai")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TrangThai")
                        .HasColumnType("bit");

                    b.Property<string>("VaiTro")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MaNV");

                    b.ToTable("NhanViens");

                    b.HasData(
                        new
                        {
                            MaNV = "NV001",
                            Email = "a.nguyen@example.com",
                            HoTen = "Nguyễn Văn A",
                            SoDienThoai = "0911123456",
                            TrangThai = true,
                            VaiTro = "Nhân viên"
                        },
                        new
                        {
                            MaNV = "NV002",
                            Email = "b.tran@example.com",
                            HoTen = "Trần Thị B",
                            SoDienThoai = "0912234567",
                            TrangThai = true,
                            VaiTro = "Quản lý kho"
                        },
                        new
                        {
                            MaNV = "NV003",
                            Email = "c.le@example.com",
                            HoTen = "Lê Văn C",
                            SoDienThoai = "0913345678",
                            TrangThai = false,
                            VaiTro = "Nhân viên"
                        });
                });

            modelBuilder.Entity("DuAnBanBanhKeo.Data.Entities.PhieuNhap", b =>
                {
                    b.Property<string>("MaPhieuNhap")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int?>("MaNCC")
                        .HasColumnType("int");

                    b.Property<DateTime>("NgayNhap")
                        .HasColumnType("datetime2");

                    b.Property<string>("NhaCungCapMaNCC")
                        .HasColumnType("nvarchar(450)");

                    b.Property<decimal>("TongTien")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("MaPhieuNhap");

                    b.HasIndex("NhaCungCapMaNCC");

                    b.ToTable("PhieuNhaps");
                });

            modelBuilder.Entity("DuAnBanBanhKeo.Data.Entities.SanPham", b =>
                {
                    b.Property<string>("MaSP")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("DonViTinh")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("GiaBan")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("GiaNhap")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("HinhAnh")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MaNCC")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("SoLuongTon")
                        .HasColumnType("int");

                    b.Property<string>("TenSP")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TrangThai")
                        .HasColumnType("bit");

                    b.HasKey("MaSP");

                    b.HasIndex("MaNCC");

                    b.ToTable("SanPhams");

                    b.HasData(
                        new
                        {
                            MaSP = "SP001",
                            DonViTinh = "Lon",
                            GiaBan = 10000m,
                            GiaNhap = 5000m,
                            MaNCC = "NCC001",
                            SoLuongTon = 100,
                            TenSP = "Coca-Cola 330ml",
                            TrangThai = true
                        },
                        new
                        {
                            MaSP = "SP002",
                            DonViTinh = "Lon",
                            GiaBan = 9500m,
                            GiaNhap = 4800m,
                            MaNCC = "NCC002",
                            SoLuongTon = 120,
                            TenSP = "Pepsi 330ml",
                            TrangThai = true
                        },
                        new
                        {
                            MaSP = "SP003",
                            DonViTinh = "Gói",
                            GiaBan = 15000m,
                            GiaNhap = 7000m,
                            MaNCC = "NCC003",
                            SoLuongTon = 50,
                            TenSP = "Bánh Oreo",
                            TrangThai = true
                        },
                        new
                        {
                            MaSP = "SP004",
                            DonViTinh = "Gói",
                            GiaBan = 18000m,
                            GiaNhap = 9000m,
                            MaNCC = "NCC001",
                            SoLuongTon = 75,
                            TenSP = "Snack Lay's",
                            TrangThai = true
                        });
                });

            modelBuilder.Entity("DuAnBanBanhKeo.Data.Entities.TaiKhoan", b =>
                {
                    b.Property<string>("MaTK")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("MaNV")
                        .IsRequired()
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("MatKhau")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TenDangNhap")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TrangThai")
                        .HasColumnType("bit");

                    b.HasKey("MaTK");

                    b.HasIndex("MaNV")
                        .IsUnique();

                    b.ToTable("TaiKhoans");
                });

            modelBuilder.Entity("DuAnBanBanhKeo.Data.Entities.ChiTietHoaDonNhap", b =>
                {
                    b.HasOne("DuAnBanBanhKeo.Data.Entities.HoaDonNhap", "HoaDonNhap")
                        .WithMany("ChiTietHoaDonNhaps")
                        .HasForeignKey("MaHoaDonNhap")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DuAnBanBanhKeo.Data.Entities.SanPham", "SanPham")
                        .WithMany("ChiTietHoaDonNhaps")
                        .HasForeignKey("MaSP")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("HoaDonNhap");

                    b.Navigation("SanPham");
                });

            modelBuilder.Entity("DuAnBanBanhKeo.Data.Entities.ChiTietHoaDonXuat", b =>
                {
                    b.HasOne("DuAnBanBanhKeo.Data.Entities.HoaDonXuat", "HoaDonXuat")
                        .WithMany("ChiTietHoaDonXuat")
                        .HasForeignKey("MaHoaDonXuat")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DuAnBanBanhKeo.Data.Entities.SanPham", "SanPham")
                        .WithMany("ChiTietHoaDonXuats")
                        .HasForeignKey("MaSP")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("HoaDonXuat");

                    b.Navigation("SanPham");
                });

            modelBuilder.Entity("DuAnBanBanhKeo.Data.Entities.ChiTietKiemKe", b =>
                {
                    b.HasOne("DuAnBanBanhKeo.Data.Entities.KiemKe", "KiemKe")
                        .WithMany("ChiTietKiemKes")
                        .HasForeignKey("MaKiemKe")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DuAnBanBanhKeo.Data.Entities.SanPham", "SanPham")
                        .WithMany("ChiTietKiemKes")
                        .HasForeignKey("MaSP")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("KiemKe");

                    b.Navigation("SanPham");
                });

            modelBuilder.Entity("DuAnBanBanhKeo.Data.Entities.ChiTietPhieuNhap", b =>
                {
                    b.HasOne("DuAnBanBanhKeo.Data.Entities.PhieuNhap", "PhieuNhap")
                        .WithMany("ChiTietPhieuNhaps")
                        .HasForeignKey("MaPhieuNhap")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DuAnBanBanhKeo.Data.Entities.SanPham", "SanPham")
                        .WithMany("ChiTietPhieuNhaps")
                        .HasForeignKey("MaSP")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PhieuNhap");

                    b.Navigation("SanPham");
                });

            modelBuilder.Entity("DuAnBanBanhKeo.Data.Entities.HoaDonNhap", b =>
                {
                    b.HasOne("DuAnBanBanhKeo.Data.Entities.NhaCungCap", "NhaCungCap")
                        .WithMany("HoaDonNhaps")
                        .HasForeignKey("MaNCC")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DuAnBanBanhKeo.Data.Entities.NhanVien", "NhanVien")
                        .WithMany("HoaDonNhaps")
                        .HasForeignKey("MaNV")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("NhaCungCap");

                    b.Navigation("NhanVien");
                });

            modelBuilder.Entity("DuAnBanBanhKeo.Data.Entities.HoaDonXuat", b =>
                {
                    b.HasOne("DuAnBanBanhKeo.Data.Entities.KhachHang", "KhachHang")
                        .WithMany("HoaDonXuats")
                        .HasForeignKey("MaKH")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DuAnBanBanhKeo.Data.Entities.NhanVien", "NhanVien")
                        .WithMany("HoaDonXuats")
                        .HasForeignKey("MaNV")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("KhachHang");

                    b.Navigation("NhanVien");
                });

            modelBuilder.Entity("DuAnBanBanhKeo.Data.Entities.KiemKe", b =>
                {
                    b.HasOne("DuAnBanBanhKeo.Data.Entities.NhanVien", "NhanVien")
                        .WithMany("KiemKes")
                        .HasForeignKey("MaNV")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("NhanVien");
                });

            modelBuilder.Entity("DuAnBanBanhKeo.Data.Entities.PhieuNhap", b =>
                {
                    b.HasOne("DuAnBanBanhKeo.Data.Entities.NhaCungCap", "NhaCungCap")
                        .WithMany()
                        .HasForeignKey("NhaCungCapMaNCC");

                    b.Navigation("NhaCungCap");
                });

            modelBuilder.Entity("DuAnBanBanhKeo.Data.Entities.SanPham", b =>
                {
                    b.HasOne("DuAnBanBanhKeo.Data.Entities.NhaCungCap", "NhaCungCap")
                        .WithMany("SanPhams")
                        .HasForeignKey("MaNCC");

                    b.Navigation("NhaCungCap");
                });

            modelBuilder.Entity("DuAnBanBanhKeo.Data.Entities.TaiKhoan", b =>
                {
                    b.HasOne("DuAnBanBanhKeo.Data.Entities.NhanVien", "NhanVien")
                        .WithOne("TaiKhoan")
                        .HasForeignKey("DuAnBanBanhKeo.Data.Entities.TaiKhoan", "MaNV");

                    b.Navigation("NhanVien");
                });

            modelBuilder.Entity("DuAnBanBanhKeo.Data.Entities.HoaDonNhap", b =>
                {
                    b.Navigation("ChiTietHoaDonNhaps");
                });

            modelBuilder.Entity("DuAnBanBanhKeo.Data.Entities.HoaDonXuat", b =>
                {
                    b.Navigation("ChiTietHoaDonXuat");
                });

            modelBuilder.Entity("DuAnBanBanhKeo.Data.Entities.KhachHang", b =>
                {
                    b.Navigation("HoaDonXuats");
                });

            modelBuilder.Entity("DuAnBanBanhKeo.Data.Entities.KiemKe", b =>
                {
                    b.Navigation("ChiTietKiemKes");
                });

            modelBuilder.Entity("DuAnBanBanhKeo.Data.Entities.NhaCungCap", b =>
                {
                    b.Navigation("HoaDonNhaps");

                    b.Navigation("SanPhams");
                });

            modelBuilder.Entity("DuAnBanBanhKeo.Data.Entities.NhanVien", b =>
                {
                    b.Navigation("HoaDonNhaps");

                    b.Navigation("HoaDonXuats");

                    b.Navigation("KiemKes");

                    b.Navigation("TaiKhoan");
                });

            modelBuilder.Entity("DuAnBanBanhKeo.Data.Entities.PhieuNhap", b =>
                {
                    b.Navigation("ChiTietPhieuNhaps");
                });

            modelBuilder.Entity("DuAnBanBanhKeo.Data.Entities.SanPham", b =>
                {
                    b.Navigation("ChiTietHoaDonNhaps");

                    b.Navigation("ChiTietHoaDonXuats");

                    b.Navigation("ChiTietKiemKes");

                    b.Navigation("ChiTietPhieuNhaps");
                });
#pragma warning restore 612, 618
        }
    }
}
