﻿using System.ComponentModel.DataAnnotations;

namespace DuAnBanBanhKeo.Data.Entities
{
    public class SanPham
    {
        [Key]
        public string? MaSP { get; set; }
        public string? TenSP { get; set; } = string.Empty;
        public decimal GiaNhap { get; set; }
        public decimal GiaBan { get; set; }
        public DateTime? NgayNhap { get; set; }
        public DateTime? NgayHetHan { get; set;  }
        public int? SoLuongTon { get; set; }
        public string? DonViTinh { get; set; }
        public bool? TrangThai { get; set; } = true;

        //Sẽ có 3 trạng thái
        //0: Hết hàng
        //1: Còn hàng
        //2: Sắp hết hàng (Số lượng tồn dưới 10 thì sẽ có tình trạng này)

        public string? MaNCC { get; set; }
        public NhaCungCap? NhaCungCap { get; set; }
        public string? MaDanhMuc { get; set; }
        public DanhMuc? DanhMuc { get; set; }

        public ICollection<ChiTietHoaDonXuat> ChiTietHoaDonXuats { get; set; } = new List<ChiTietHoaDonXuat>();
        public ICollection<ChiTietHoaDonNhap> ChiTietHoaDonNhaps { get; set; } = new List<ChiTietHoaDonNhap>();
        public ICollection<ChiTietKiemKe> ChiTietKiemKes { get; set; } = new List<ChiTietKiemKe>();
        public ICollection<ChiTietPhieuNhap> ChiTietPhieuNhaps { get; set; } = new List<ChiTietPhieuNhap>();
        public ICollection<HinhAnh> HinhAnhs { get; set; } = new List<HinhAnh>(); // Navigation property for HinhAnh
    }
}