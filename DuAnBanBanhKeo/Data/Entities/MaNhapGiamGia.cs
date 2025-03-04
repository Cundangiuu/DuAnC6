using System.ComponentModel.DataAnnotations;

namespace DuAnBanBanhKeo.Data.Entities
{
    public class MaNhapGiamGia
    {
        [Key]
        public int Id { get; set; } // Khóa chính cho bảng mã nhập chi tiết

        [Required]
        public string MaNhap { get; set; } = null!; // Mã nhập chi tiết
        public string MaGiamGia { get; set; } = null!;
        public bool IsUsed { get; set; } = false;
        public virtual GiamGia GiamGia { get; set; } = null!;
    }
}
