using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DuAnBanBanhKeo.Data.Entities
{
    public class KiemKe
    {
        [Key]
        public string MaKiemKe { get; set; } // Sử dụng kiểu int và để database tự tạo

        [Required]
        public DateTime NgayKiemKe { get; set; } = DateTime.Now;

        [Required] // Thêm [Required] ở đây
        public string MaNV { get; set; }

        [ForeignKey("MaNV")]
        public NhanVien NhanVien { get; set; } // Nhân viên thực hiện kiểm kê

        public string GhiChu { get; set; }

        public ICollection<ChiTietKiemKe> ChiTietKiemKes { get; set; } = new List<ChiTietKiemKe>();
    }
}