namespace DuAnBanBanhKeo.Modal
{
    public class HoaDonNhapUpdateDto
    {
        public string MaHoaDonNhap { get; set; }
        public string MaNCC { get; set; }
        public string TenNCC { get; set; }
        public decimal TongTien { get; set; }
        public int TrangThai { get; set; }
        public List<ChiTietHoaDonNhapDto> ChiTietHoaDonNhaps { get; set; }

    }
}
