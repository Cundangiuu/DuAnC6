using System.ComponentModel.DataAnnotations;

namespace DuAnBanBanhKeo.Data.Entities
{
    public class HoaDonNhap
    {
        [Key]
        public string MaHoaDonNhap { get; set; }

        public DateTime NgayNhap { get; set; } = DateTime.Now;

        public string MaNV { get; set; }
        public NhanVien NhanVien { get; set; } // Nhân viên nhập hàng

        public string MaNCC { get; set; }
        public NhaCungCap NhaCungCap { get; set; } // Nhà cung cấp

        public decimal TongTien { get; set; } // Tổng tiền nhập hàng

        public ICollection<ChiTietHoaDonNhap> ChiTietHoaDonNhaps { get; set; } = new List<ChiTietHoaDonNhap>();
    }
}
