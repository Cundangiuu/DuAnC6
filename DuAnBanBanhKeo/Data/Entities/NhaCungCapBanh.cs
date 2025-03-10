using System.ComponentModel.DataAnnotations;

namespace DuAnBanBanhKeo.Data.Entities
{
    public class NhaCungCapBanh
    {
        [Key]
        public int MaNhaCungCapBanh { get; set; }
        public string TenNhaCungCap { get; set; }
        public ICollection<SanPham> SanPhams { get; set; }
    }
}
