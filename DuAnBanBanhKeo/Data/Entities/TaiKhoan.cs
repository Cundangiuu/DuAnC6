using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DuAnBanBanhKeo.Data.Entities
{
    public class TaiKhoan
    {
        [Key]
        public string MaTK { get; set; }
<<<<<<< Updated upstream
        public string TenDangNhap { get; set; }
        public string MatKhau { get; set; } 
        public string MaNV { get; set; }
        public NhanVien NhanVien { get; set; }
        public bool TrangThai { get; set; }
    }
=======
>>>>>>> Stashed changes

        [Required(ErrorMessage = "Tên đăng nhập là bắt buộc.")]
        public string TenDangNhap { get; set; }

        [Required(ErrorMessage = "Mật khẩu là bắt buộc.")]
        public string MatKhau { get; set; } // Mã hóa mật khẩu khi lưu

        [Required(ErrorMessage = "Mã nhân viên là bắt buộc.")]
        public string MaNV { get; set; }

        [JsonIgnore]
        [Required(ErrorMessage = "Nhân viên là bắt buộc.")]
        public NhanVien NhanVien { get; set; }

        public bool TrangThai { get; set; } = true;
    }
}