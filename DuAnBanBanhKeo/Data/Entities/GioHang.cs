using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DuAnBanBanhKeo.Data.Entities
{
    public class GioHang
    {
        [Key]
        public string MaGioHang { get; set; } = null!;

        public string? MaKhachHang { get; set; }

        public DateOnly NgayTao { get; set; }

        [ForeignKey("MaKhachHang")]
        public virtual KhachHang? KhachHang { get; set; }

        public virtual ICollection<ChiTietGioHang> ChiTietGioHangs { get; set; } = new List<ChiTietGioHang>();
    }
}
