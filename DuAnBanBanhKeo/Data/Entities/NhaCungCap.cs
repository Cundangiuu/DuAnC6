using System.ComponentModel.DataAnnotations;

namespace DuAnBanBanhKeo.Data.Entities
{
    public class NhaCungCap
    {
        [Key]
        public string MaNCC { get; set; }
        public string TenNCC { get; set; } = string.Empty;
        public string? DiaChi { get; set; }
        public string? SoDienThoai { get; set; }
        public string? Email { get; set; }
        public ICollection<HoaDonNhap> HoaDonNhaps { get; set; } = new List<HoaDonNhap>();
        public ICollection<SanPham> SanPhams { get; set; } = new List<SanPham>();
    }
}
