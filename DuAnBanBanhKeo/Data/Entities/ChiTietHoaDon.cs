using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DuAnBanBanhKeo.Data.Entities
{
    public class ChiTietHoaDon
    {
        [Key]
        public int Id { get; set; }

        public string MaHoaDon { get; set; } = null!;

        public string? MaSanPham { get; set; }

        public string? MaCombo { get; set; }

        public int SoLuong { get; set; }

        public decimal Gia { get; set; }

        [ForeignKey("MaHoaDon")]
        public virtual HoaDon HoaDon { get; set; }

        [ForeignKey("MaSanPham")]
        public virtual SanPham? SanPham { get; set; }

        [ForeignKey("MaCombo")]
        public virtual Combo? Combo { get; set; }
    }
}
