using System.ComponentModel.DataAnnotations;

namespace DuAnBanBanhKeo.Data.Entities
{
    public class KhachHang
    {
        [Key]
        public string MaKH { get; set; } = string.Empty;
        public string TenKH { get; set; } = string.Empty;

        public string? SoDienThoai { get; set; }
        public string? Email { get; set; }
        public string? DiaChi { get; set; }

        public ICollection<HoaDonXuat> HoaDonXuats { get; set; } = new List<HoaDonXuat>();
    }
}