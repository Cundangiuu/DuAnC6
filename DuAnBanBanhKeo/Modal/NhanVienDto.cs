// NhanVienDto.cs
using Microsoft.AspNetCore.Http;

namespace DuAnBanBanhKeo.Modal
{
    public class NhanVienDto
    {
        public string MaNV { get; set; }
        public string? HinhAnh { get; set; }
        public string HoTen { get; set; }
        public string VaiTro { get; set; }
        public string SoDienThoai { get; set; }
        public string Email { get; set; }
        public bool TrangThai { get; set; }
        public string TenDangNhap { get; set; }
        public string MatKhau { get; set; }
        public bool TrangThaiTK { get; set; }
        public IFormFile? HinhAnhFile { get; set; }
    }

    public class NhanVienUpdateDto
    {
        public string MaNV { get; set; }
        public string? HinhAnh { get; set; }
        public string HoTen { get; set; }
        public string VaiTro { get; set; }
        public string SoDienThoai { get; set; }
        public string Email { get; set; }
        public bool TrangThai { get; set; }
        public IFormFile? HinhAnhFile { get; set; }
    }
}