using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DuAnBanBanhKeo.Data.Entities
{
    public class ChiTietGioHang
    {
        [Key]
        public int Id { get; set; }

        public string MaGioHang { get; set; } = null!;

        [ForeignKey("MaGioHang")]
        public virtual GioHang GioHang { get; set; }

        public string? MaSanPham { get; set; } // Cho phép null

        [ForeignKey("MaSanPham")]
        public virtual SanPham? SanPham { get; set; } // Cho phép null

        public string? MaCombo { get; set; }

        [ForeignKey("MaCombo")]
        public virtual Combo? Combo { get; set; }

        public int SoLuong { get; set; }
    }
}
