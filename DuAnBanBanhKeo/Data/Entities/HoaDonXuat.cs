using System.ComponentModel.DataAnnotations;

namespace DuAnBanBanhKeo.Data.Entities
{
    public class HoaDonXuat
    {
        [Key]
        public string MaHoaDonXuat { get; set; }
        public DateTime NgayXuat { get; set; } = DateTime.Now;

        public string MaNV { get; set; }
        public NhanVien NhanVien { get; set; } // Nhân viên bán hàng

        public string MaKH { get; set; }
        public KhachHang KhachHang { get; set; } // Khách hàng mua hàng

        public decimal TongTien { get; set; } // Tổng tiền bán hàng

        public ICollection<ChiTietHoaDonXuat> ChiTietHoaDonXuat { get; set; } = new List<ChiTietHoaDonXuat>();
    }
}
