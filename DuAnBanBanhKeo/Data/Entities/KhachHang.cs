using DuAnBanBanhKeo.Api.Data.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DuAnBanBanhKeo.Data.Entities
{
    public class KhachHang
    {
        [Key]
        public string MaKhachHang { get; set; } = null!;

        public string TenKhachHang { get; set; } = null!;
        public string? SoDienThoai { get; set; }

        public string? DiaChi { get; set; }
        public DateOnly NgayDangKy { get; set; }

        public string? HinhAnh { get; set; }

        [NotMapped]
        public IFormFile Anh { get; set; }

        public string? Cccd { get; set; }

        public DateOnly? NgaySinh { get; set; }

        public bool? GioiTinh { get; set; }

        public int? TrangThai { get; set; }

        public virtual ICollection<DonHang> DonHangs { get; set; } = new List<DonHang>();

        public virtual ICollection<GioHang> GioHangs { get; set; } = new List<GioHang>();

        public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();

        public virtual ICollection<TaiKhoan> TaiKhoans { get; set; } = new List<TaiKhoan>();
    }
}
