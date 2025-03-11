using System.ComponentModel.DataAnnotations;

namespace DuAnBanBanhKeo.Data.Entities
{
    public class ChiTietHoaDonXuat
    {
        [Key]
        public int Id { get; set; }

        public string MaHoaDonXuat { get; set; }
        public HoaDonXuat HoaDonXuat { get; set; }

        public string MaSP { get; set; }
        public SanPham SanPham { get; set; }

        public int SoLuong { get; set; } // Số lượng bán
        public decimal DonGia { get; set; } // Giá bán
        public decimal ThanhTien => SoLuong * DonGia; // Thành tiền
    }
}
