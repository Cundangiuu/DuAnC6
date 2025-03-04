using DuAnBanBanhKeo.Api.Data.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DuAnBanBanhKeo.Data.Entities
{
    public class NhanVien
    {
        [Key]
        public string MaNhanVien { get; set; } = null!;

        public string TenNhanVien { get; set; } = null!;

        public string VaiTro { get; set; } = null!;

        public string? SoDienThoai { get; set; }

        public DateOnly? NgayBatDau { get; set; }

        public string? HinhAnh { get; set; }

        public DateOnly? NgaySinh { get; set; }

        public string? DiaChi { get; set; }

        [NotMapped]
        public IFormFile Anh { get; set; }
        public int? TrangThai { get; set; }

        public string? Cccd { get; set; }

        public bool? GioiTinh { get; set; }

        public virtual ICollection<TaiKhoan> TaiKhoans { get; set; } = new List<TaiKhoan>();
    }
}
