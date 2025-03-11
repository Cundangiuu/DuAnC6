using System.ComponentModel.DataAnnotations;

namespace DuAnBanBanhKeo.Data.Entities
{
    public class ChiTietHoaDonNhap
    {
        [Key]
        public int Id { get; set; }

        public string MaHoaDonNhap { get; set; }
        public HoaDonNhap HoaDonNhap { get; set; }

        public string MaSP { get; set; }
        public SanPham SanPham { get; set; }

        public int SoLuong { get; set; } // Số lượng nhập
        public decimal DonGia { get; set; } // Giá nhập
        public decimal ThanhTien => SoLuong * DonGia; // Thành tiền
    }
}
