using System.ComponentModel.DataAnnotations;

namespace DuAnBanBanhKeo.Data.Entities
{
    public class NhaCungCap
    {
        [Key]
        public string MaNhaCungCap { get; set; } = null!;
        public string TenNhaCungCap { get; set; } = null!; // Tên nhà cung cấp
        public int? TrangThai { get; set; }
        public ICollection<SanPham> SanPhams { get; set; } // Danh sách sản 
    }
}
