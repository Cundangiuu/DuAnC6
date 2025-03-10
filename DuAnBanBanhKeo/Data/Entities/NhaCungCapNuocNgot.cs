using System.ComponentModel.DataAnnotations;

namespace DuAnBanBanhKeo.Data.Entities
{
    public class NhaCungCapNuocNgot
    {
        [Key]
        public int MaNhaCungCapNuocNgot { get; set; }
        public string TenNhaCungCap { get; set; }
        public ICollection<SanPham> SanPhams { get; set; }
    }
}
