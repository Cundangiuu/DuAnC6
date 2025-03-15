using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace DuAnBanBanhKeo.Data.Entities
{
    public class NhanVien
    {
        [Key]
        [Required(ErrorMessage = "Mã nhân viên là bắt buộc.")]
        [StringLength(10, ErrorMessage = "Mã nhân viên không được vượt quá 10 ký tự.")]
        public string MaNV { get; set; }
        public string? HinhAnh { get; set; }
<<<<<<< Updated upstream
=======
        [Required(ErrorMessage = "Họ tên là bắt buộc.")]
        [StringLength(100, ErrorMessage = "Họ tên không được vượt quá 100 ký tự.")]
>>>>>>> Stashed changes
        public string HoTen { get; set; }

        public string VaiTro { get; set; }

<<<<<<< Updated upstream
        public string SoDienThoai { get; set; }

=======
        [Required(ErrorMessage = "Số điện thoại là bắt buộc.")]
        [RegularExpression(@"^0[0-9]{9}$", ErrorMessage = "Số điện thoại không hợp lệ.")]
        public string SoDienThoai { get; set; }

        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
>>>>>>> Stashed changes
        public string Email { get; set; }
        public ICollection<KiemKe> KiemKes { get; set; } = new List<KiemKe>();
        public bool TrangThai { get; set; } = true;
        public TaiKhoan? TaiKhoan { get; set; }
        public ICollection<HoaDonXuat> HoaDonXuats { get; set; } = new List<HoaDonXuat>();
        public ICollection<HoaDonNhap> HoaDonNhaps { get; set; } = new List<HoaDonNhap>();
    }
}