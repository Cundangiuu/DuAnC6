using System.ComponentModel.DataAnnotations;

namespace DuAnBanBanhKeo.Data.Entities
{
    public class ChiTietKiemKe
    {
        [Key]
        public int Id { get; set; }
        public string MaKiemKe { get; set; }
        public KiemKe KiemKe { get; set; }

        public string MaSP { get; set; }
        public SanPham SanPham { get; set; }

        public int SoLuongThucTe { get; set; } // Số lượng kiểm kê thực tế
        public string GhiChu { get; set; } = string.Empty; // Ghi chú nếu có sai lệch
    }

}
