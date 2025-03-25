namespace DuAnBanBanhKeo.Modal
{
    public class ChiTietKiemKeViewModel
    {
        public string MaSP { get; set; }
        public string TenSanPham { get; set; }
        public int SoLuongTonKho { get; set; }
        public int SoLuongThucTe { get; set; }
        public int ChenhLechSoLuong { get; set; } // Thêm trường chênh lệch
        public string GhiChu { get; set; }
    }
}