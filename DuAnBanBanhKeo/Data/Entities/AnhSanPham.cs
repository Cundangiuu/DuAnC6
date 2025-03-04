using System.ComponentModel.DataAnnotations;

namespace DuAnBanBanhKeo.Data.Entities
{
    public class AnhSanPham
    {
        [Key]
        public int Id { get; set; }

        public string MaSanPham { get; set; } = null!;

        public string UrlAnh { get; set; } = null!;

        public virtual SanPham MaSanPhamNavigation { get; set; } = null!;
    }
}
