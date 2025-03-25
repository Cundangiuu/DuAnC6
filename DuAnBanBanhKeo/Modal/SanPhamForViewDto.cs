namespace DuAnBanBanhKeo.Modal
{
    public class SanPhamForViewDto
    {
        public string? MaSP { get; set; } // Giữ lại MaSP nếu cần thiết cho việc thao tác
        public string? TenSP { get; set; }
        public string? HinhAnhDaiDien { get; set; } 
        public string? TenDanhMuc { get; set; }
        public string? TenNCC { get; set; }
        public int? SoLuongTon { get; set; }
        public string? TrangThai { get; set; } // Trạng thái hiển thị (ví dụ: "Còn hàng", "Hết hàng")
    }
}