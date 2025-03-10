using System.ComponentModel.DataAnnotations;

namespace DuAnBanBanhKeo.Data.Entities
{
    public class NhaCungCapKeo
    {
        [Key]
        public int MaNhaCungCapKeo { get; set; }
        public string TenNhaCungCap { get; set; }
        public ICollection<SanPham> SanPhams { get; set; }
    }
}
