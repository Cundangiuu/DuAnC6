using DuAnBanBanhKeo.Data.Entities;

namespace DuAnBanBanhKeo.Modal
{
    public class HoaDonNhapDto
    {
        public string MaHoaDonNhap { get; set; }

        public DateTime NgayNhap { get; set; } = DateTime.Now;

        public string MaNV { get; set; }
        public string HoTen { get; set; }

        public string MaNCC { get; set; }
        public string TenNCC { get; set; }

        public decimal TongTien { get; set; } // Tổng tiền nhập hàng
        public int TrangThai { get; set; }
        public List<ChiTietHoaDonNhapDto> ChiTietHoaDonNhaps { get; set; }
    }
}
