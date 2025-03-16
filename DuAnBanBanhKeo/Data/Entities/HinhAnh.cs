using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DuAnBanBanhKeo.Data.Entities
{
    public class HinhAnh
    {
        [Key]
        public int HinhAnhId { get; set; } // Khóa chính, tự tăng

        [Required]
        [MaxLength(255)] // Điều chỉnh độ dài tối đa của URL nếu cần
        public string Url { get; set; } = string.Empty; // Đường dẫn hoặc URL của hình ảnh

        
         // Độ dài tối đa của MaSP
        public string MaSP { get; set; } // Khóa ngoại liên kết đến SanPham

        [ForeignKey("MaSP")]
        public SanPham? SanPham { get; set; } // Navigation property đến SanPham
    }
}