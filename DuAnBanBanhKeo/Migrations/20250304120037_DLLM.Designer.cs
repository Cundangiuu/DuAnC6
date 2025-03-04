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
    [Migration("20250304120037_DLLM")]
    partial class DLLM
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DuAnBanBanhKeo.Api.Data.Entities.TaiKhoan", b =>
                {
                    b.Property<string>("MaTaiKhoan")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MaKhachHang")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("MaNhanVien")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MaNhanVienNavigationMaNhanVien")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("MatKhau")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TenTaiKhoan")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TinhTrang")
                        .HasColumnType("int");

                    b.Property<string>("VaiTro")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MaTaiKhoan");

                    b.HasIndex("MaKhachHang");

                    b.HasIndex("MaNhanVienNavigationMaNhanVien");

                    b.ToTable("TaiKhoans");
                });

            modelBuilder.Entity("DuAnBanBanhKeo.Data.Entities.AnhSanPham", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("MaSanPham")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("UrlAnh")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("MaSanPham");

                    b.ToTable("AnhSanPhams");
                });

            modelBuilder.Entity("DuAnBanBanhKeo.Data.Entities.ChiTietCombo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("MaCombo")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("MaSanPham")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("SoLuong")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("MaCombo");

                    b.HasIndex("MaSanPham");

                    b.ToTable("ChiTietCombos");
                });

            modelBuilder.Entity("DuAnBanBanhKeo.Data.Entities.ChiTietDonHang", b =>
                {
                    b.Property<int>("MaChiTietDonHang")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MaChiTietDonHang"));

                    b.Property<decimal>("Gia")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("MaCombo")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("MaDonHang")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("MaSanPham")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("SoLuong")
                        .HasColumnType("int");

                    b.HasKey("MaChiTietDonHang");

                    b.HasIndex("MaCombo");

                    b.HasIndex("MaDonHang");

                    b.HasIndex("MaSanPham");

                    b.ToTable("ChiTietDonHangs");
                });

            modelBuilder.Entity("DuAnBanBanhKeo.Data.Entities.ChiTietGioHang", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("MaCombo")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("MaGioHang")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("MaSanPham")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("SoLuong")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("MaCombo");

                    b.HasIndex("MaGioHang");

                    b.HasIndex("MaSanPham");

                    b.ToTable("ChiTietGioHangs");
                });

            modelBuilder.Entity("DuAnBanBanhKeo.Data.Entities.ChiTietHoaDon", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Gia")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("MaCombo")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("MaHoaDon")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("MaSanPham")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("SoLuong")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("MaCombo");

                    b.HasIndex("MaHoaDon");

                    b.HasIndex("MaSanPham");

                    b.ToTable("ChiTietHoaDons");
                });

            modelBuilder.Entity("DuAnBanBanhKeo.Data.Entities.Combo", b =>
                {
                    b.Property<string>("MaCombo")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Anh")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Gia")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("MoTa")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TenCombo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TrangThai")
                        .HasColumnType("int");

                    b.HasKey("MaCombo");

                    b.ToTable("Combos");
                });

            modelBuilder.Entity("DuAnBanBanhKeo.Data.Entities.DonHang", b =>
                {
                    b.Property<string>("MaDonHang")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("MaKhachHang")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateOnly>("NgayDatHang")
                        .HasColumnType("date");

                    b.Property<decimal>("TongTien")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("TrangThai")
                        .HasColumnType("int");

                    b.HasKey("MaDonHang");

                    b.HasIndex("MaKhachHang");

                    b.ToTable("DonHangs");
                });

            modelBuilder.Entity("DuAnBanBanhKeo.Data.Entities.GiamGia", b =>
                {
                    b.Property<string>("MaGiamGia")
                        .HasColumnType("nvarchar(450)");

                    b.Property<decimal>("GiaTri")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateOnly>("NgayBatDau")
                        .HasColumnType("date");

                    b.Property<DateOnly>("NgayKetThuc")
                        .HasColumnType("date");

                    b.Property<int>("SoLuongMaNhapToiDa")
                        .HasColumnType("int");

                    b.Property<string>("TenGiamGia")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TrangThai")
                        .HasColumnType("int");

                    b.HasKey("MaGiamGia");

                    b.ToTable("GiamGias");
                });

            modelBuilder.Entity("DuAnBanBanhKeo.Data.Entities.GioHang", b =>
                {
                    b.Property<string>("MaGioHang")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("MaKhachHang")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateOnly>("NgayTao")
                        .HasColumnType("date");

                    b.HasKey("MaGioHang");

                    b.HasIndex("MaKhachHang");

                    b.ToTable("GioHangs");
                });

            modelBuilder.Entity("DuAnBanBanhKeo.Data.Entities.HoaDon", b =>
                {
                    b.Property<string>("MaHoaDon")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("MaGiamGia")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("MaKhachHang")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateOnly>("NgayXuatHoaDon")
                        .HasColumnType("date");

                    b.Property<decimal>("TongTien")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("TrangThai")
                        .HasColumnType("int");

                    b.HasKey("MaHoaDon");

                    b.HasIndex("MaGiamGia");

                    b.HasIndex("MaKhachHang");

                    b.ToTable("HoaDons");
                });

            modelBuilder.Entity("DuAnBanBanhKeo.Data.Entities.KhachHang", b =>
                {
                    b.Property<string>("MaKhachHang")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Cccd")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DiaChi")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("GioiTinh")
                        .HasColumnType("bit");

                    b.Property<string>("HinhAnh")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateOnly>("NgayDangKy")
                        .HasColumnType("date");

                    b.Property<DateOnly?>("NgaySinh")
                        .HasColumnType("date");

                    b.Property<string>("SoDienThoai")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TenKhachHang")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("TrangThai")
                        .HasColumnType("int");

                    b.HasKey("MaKhachHang");

                    b.ToTable("KhachHangs");
                });

            modelBuilder.Entity("DuAnBanBanhKeo.Data.Entities.MaNhapGiamGia", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("IsUsed")
                        .HasColumnType("bit");

                    b.Property<string>("MaGiamGia")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("MaNhap")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("MaGiamGia");

                    b.ToTable("MaNhapGiamGias");
                });

            modelBuilder.Entity("DuAnBanBanhKeo.Data.Entities.NhaCungCap", b =>
                {
                    b.Property<string>("MaNhaCungCap")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("TenNhaCungCap")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("TrangThai")
                        .HasColumnType("int");

                    b.HasKey("MaNhaCungCap");

                    b.ToTable("NhaCungCaps");
                });

            modelBuilder.Entity("DuAnBanBanhKeo.Data.Entities.NhanVien", b =>
                {
                    b.Property<string>("MaNhanVien")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Cccd")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DiaChi")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("GioiTinh")
                        .HasColumnType("bit");

                    b.Property<string>("HinhAnh")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateOnly?>("NgayBatDau")
                        .HasColumnType("date");

                    b.Property<DateOnly?>("NgaySinh")
                        .HasColumnType("date");

                    b.Property<string>("SoDienThoai")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TenNhanVien")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("TrangThai")
                        .HasColumnType("int");

                    b.Property<string>("VaiTro")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MaNhanVien");

                    b.ToTable("NhanViens");
                });

            modelBuilder.Entity("DuAnBanBanhKeo.Data.Entities.SanPham", b =>
                {
                    b.Property<string>("MaSanPham")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("DonVi")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Gia")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateOnly?>("Hsd")
                        .HasColumnType("date");

                    b.Property<string>("MaNhaCungCap")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("MoTa")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateOnly?>("NgayThem")
                        .HasColumnType("date");

                    b.Property<DateOnly?>("Nsx")
                        .HasColumnType("date");

                    b.Property<int>("SoLuong")
                        .HasColumnType("int");

                    b.Property<string>("TenSanPham")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("TrangThai")
                        .HasColumnType("int");

                    b.HasKey("MaSanPham");

                    b.HasIndex("MaNhaCungCap");

                    b.ToTable("SanPhams");
                });

            modelBuilder.Entity("DuAnBanBanhKeo.Api.Data.Entities.TaiKhoan", b =>
                {
                    b.HasOne("DuAnBanBanhKeo.Data.Entities.KhachHang", "MaKhachHangNavigation")
                        .WithMany("TaiKhoans")
                        .HasForeignKey("MaKhachHang");

                    b.HasOne("DuAnBanBanhKeo.Data.Entities.NhanVien", "MaNhanVienNavigation")
                        .WithMany("TaiKhoans")
                        .HasForeignKey("MaNhanVienNavigationMaNhanVien");

                    b.Navigation("MaKhachHangNavigation");

                    b.Navigation("MaNhanVienNavigation");
                });

            modelBuilder.Entity("DuAnBanBanhKeo.Data.Entities.AnhSanPham", b =>
                {
                    b.HasOne("DuAnBanBanhKeo.Data.Entities.SanPham", "MaSanPhamNavigation")
                        .WithMany("AnhSanPhams")
                        .HasForeignKey("MaSanPham")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MaSanPhamNavigation");
                });

            modelBuilder.Entity("DuAnBanBanhKeo.Data.Entities.ChiTietCombo", b =>
                {
                    b.HasOne("DuAnBanBanhKeo.Data.Entities.Combo", "MaComboNavigation")
                        .WithMany("ChiTietCombos")
                        .HasForeignKey("MaCombo")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DuAnBanBanhKeo.Data.Entities.SanPham", "MaSanPhamNavigation")
                        .WithMany("ChiTietCombos")
                        .HasForeignKey("MaSanPham")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MaComboNavigation");

                    b.Navigation("MaSanPhamNavigation");
                });

            modelBuilder.Entity("DuAnBanBanhKeo.Data.Entities.ChiTietDonHang", b =>
                {
                    b.HasOne("DuAnBanBanhKeo.Data.Entities.Combo", "Combo")
                        .WithMany("ChiTietDonHangs")
                        .HasForeignKey("MaCombo");

                    b.HasOne("DuAnBanBanhKeo.Data.Entities.DonHang", "DonHang")
                        .WithMany("ChiTietDonHangs")
                        .HasForeignKey("MaDonHang")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DuAnBanBanhKeo.Data.Entities.SanPham", "MaSanPhamNavigation")
                        .WithMany("ChiTietDonHangs")
                        .HasForeignKey("MaSanPham");

                    b.Navigation("Combo");

                    b.Navigation("DonHang");

                    b.Navigation("MaSanPhamNavigation");
                });

            modelBuilder.Entity("DuAnBanBanhKeo.Data.Entities.ChiTietGioHang", b =>
                {
                    b.HasOne("DuAnBanBanhKeo.Data.Entities.Combo", "Combo")
                        .WithMany("ChiTietGioHangs")
                        .HasForeignKey("MaCombo");

                    b.HasOne("DuAnBanBanhKeo.Data.Entities.GioHang", "GioHang")
                        .WithMany("ChiTietGioHangs")
                        .HasForeignKey("MaGioHang")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DuAnBanBanhKeo.Data.Entities.SanPham", "SanPham")
                        .WithMany("ChiTietGioHangs")
                        .HasForeignKey("MaSanPham");

                    b.Navigation("Combo");

                    b.Navigation("GioHang");

                    b.Navigation("SanPham");
                });

            modelBuilder.Entity("DuAnBanBanhKeo.Data.Entities.ChiTietHoaDon", b =>
                {
                    b.HasOne("DuAnBanBanhKeo.Data.Entities.Combo", "Combo")
                        .WithMany("ChiTietHoaDons")
                        .HasForeignKey("MaCombo");

                    b.HasOne("DuAnBanBanhKeo.Data.Entities.HoaDon", "HoaDon")
                        .WithMany("ChiTietHoaDons")
                        .HasForeignKey("MaHoaDon")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DuAnBanBanhKeo.Data.Entities.SanPham", "SanPham")
                        .WithMany("ChiTietHoaDons")
                        .HasForeignKey("MaSanPham");

                    b.Navigation("Combo");

                    b.Navigation("HoaDon");

                    b.Navigation("SanPham");
                });

            modelBuilder.Entity("DuAnBanBanhKeo.Data.Entities.DonHang", b =>
                {
                    b.HasOne("DuAnBanBanhKeo.Data.Entities.KhachHang", "KhachHang")
                        .WithMany("DonHangs")
                        .HasForeignKey("MaKhachHang")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("KhachHang");
                });

            modelBuilder.Entity("DuAnBanBanhKeo.Data.Entities.GioHang", b =>
                {
                    b.HasOne("DuAnBanBanhKeo.Data.Entities.KhachHang", "KhachHang")
                        .WithMany("GioHangs")
                        .HasForeignKey("MaKhachHang");

                    b.Navigation("KhachHang");
                });

            modelBuilder.Entity("DuAnBanBanhKeo.Data.Entities.HoaDon", b =>
                {
                    b.HasOne("DuAnBanBanhKeo.Data.Entities.GiamGia", "MaGiamGiaNavigation")
                        .WithMany("HoaDons")
                        .HasForeignKey("MaGiamGia")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("DuAnBanBanhKeo.Data.Entities.KhachHang", "KhachHang")
                        .WithMany("HoaDons")
                        .HasForeignKey("MaKhachHang");

                    b.Navigation("KhachHang");

                    b.Navigation("MaGiamGiaNavigation");
                });

            modelBuilder.Entity("DuAnBanBanhKeo.Data.Entities.MaNhapGiamGia", b =>
                {
                    b.HasOne("DuAnBanBanhKeo.Data.Entities.GiamGia", "GiamGia")
                        .WithMany("MaNhapGiamGias")
                        .HasForeignKey("MaGiamGia")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("GiamGia");
                });

            modelBuilder.Entity("DuAnBanBanhKeo.Data.Entities.SanPham", b =>
                {
                    b.HasOne("DuAnBanBanhKeo.Data.Entities.NhaCungCap", "MaNhaCungCapNavigation")
                        .WithMany("SanPhams")
                        .HasForeignKey("MaNhaCungCap");

                    b.Navigation("MaNhaCungCapNavigation");
                });

            modelBuilder.Entity("DuAnBanBanhKeo.Data.Entities.Combo", b =>
                {
                    b.Navigation("ChiTietCombos");

                    b.Navigation("ChiTietDonHangs");

                    b.Navigation("ChiTietGioHangs");

                    b.Navigation("ChiTietHoaDons");
                });

            modelBuilder.Entity("DuAnBanBanhKeo.Data.Entities.DonHang", b =>
                {
                    b.Navigation("ChiTietDonHangs");
                });

            modelBuilder.Entity("DuAnBanBanhKeo.Data.Entities.GiamGia", b =>
                {
                    b.Navigation("HoaDons");

                    b.Navigation("MaNhapGiamGias");
                });

            modelBuilder.Entity("DuAnBanBanhKeo.Data.Entities.GioHang", b =>
                {
                    b.Navigation("ChiTietGioHangs");
                });

            modelBuilder.Entity("DuAnBanBanhKeo.Data.Entities.HoaDon", b =>
                {
                    b.Navigation("ChiTietHoaDons");
                });

            modelBuilder.Entity("DuAnBanBanhKeo.Data.Entities.KhachHang", b =>
                {
                    b.Navigation("DonHangs");

                    b.Navigation("GioHangs");

                    b.Navigation("HoaDons");

                    b.Navigation("TaiKhoans");
                });

            modelBuilder.Entity("DuAnBanBanhKeo.Data.Entities.NhaCungCap", b =>
                {
                    b.Navigation("SanPhams");
                });

            modelBuilder.Entity("DuAnBanBanhKeo.Data.Entities.NhanVien", b =>
                {
                    b.Navigation("TaiKhoans");
                });

            modelBuilder.Entity("DuAnBanBanhKeo.Data.Entities.SanPham", b =>
                {
                    b.Navigation("AnhSanPhams");

                    b.Navigation("ChiTietCombos");

                    b.Navigation("ChiTietDonHangs");

                    b.Navigation("ChiTietGioHangs");

                    b.Navigation("ChiTietHoaDons");
                });
#pragma warning restore 612, 618
        }
    }
}
