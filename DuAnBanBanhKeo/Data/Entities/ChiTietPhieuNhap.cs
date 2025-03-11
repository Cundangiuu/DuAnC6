using System.ComponentModel.DataAnnotations;

namespace DuAnBanBanhKeo.Data.Entities
{
    public class ChiTietPhieuNhap
    {
        [Key]
        public int MaChiTiet { get; set; }
        public string MaPhieuNhap { get; set; }
        public PhieuNhap PhieuNhap { get; set; }
        public string MaSP { get; set; }
        public SanPham SanPham { get; set; }

        public int SoLuong { get; set; }
        public decimal GiaNhap { get; set; }
    }
}
