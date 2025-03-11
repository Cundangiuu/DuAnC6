using System.ComponentModel.DataAnnotations;

namespace DuAnBanBanhKeo.Data.Entities
{
    public class ChiTietCombo
    {
        [Key]
        public int Id { get; set; }

        public string MaCombo { get; set; } = null!;

        public string MaSanPham { get; set; } = null!;

        public int SoLuongCombo { get; set; }

        public virtual Combo MaComboNavigation { get; set; } = null!;

        public virtual SanPham MaSanPhamNavigation { get; set; } = null!;
    }
}
