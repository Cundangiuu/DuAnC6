namespace DuAnBanBanhKeo.Modal
{
    public class KiemKeViewModel
    {
        public string MaKiemKe { get; set; }
        public DateTime NgayKiemKe { get; set; }
        public string MaNV { get; set; }
        public string TenNhanVien { get; set; } // Thêm trường này
        public string GhiChu { get; set; }
        public int TrangThai { get; set; }
        public List<ChiTietKiemKeViewModel> ChiTietKiemKes { get; set; }
    }
}