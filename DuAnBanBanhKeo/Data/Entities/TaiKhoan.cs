using System.ComponentModel.DataAnnotations;

namespace DuAnBanBanhKeo.Data.Entities
{
    public class TaiKhoan
    {
        [Key]
        public string MaTK { get; set; }
        public string TenDangNhap { get; set; }
        public string MatKhau { get; set; } // Mã hóa mật khẩu khi lưu
        public string MaNV { get; set; }
        public NhanVien NhanVien { get; set; }
    }

}
