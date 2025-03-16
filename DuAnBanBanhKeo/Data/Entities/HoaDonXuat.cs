using System.ComponentModel.DataAnnotations;

namespace DuAnBanBanhKeo.Data.Entities
{
    public class HoaDonXuat
    {
        [Key]
        public string MaHoaDonXuat { get; set; }
        public DateTime NgayXuat { get; set; } = DateTime.Now;

        public string MaNV { get; set; }
        public NhanVien NhanVien { get; set; } 

        public string MaKH { get; set; }
        public KhachHang KhachHang { get; set; } 

        public decimal TongTien { get; set; } // Tổng tiền bán hàng
        public int TrangThai { get; set; } = 0;

        public ICollection<ChiTietHoaDonXuat> ChiTietHoaDonXuat { get; set; } = new List<ChiTietHoaDonXuat>();
    }
}
