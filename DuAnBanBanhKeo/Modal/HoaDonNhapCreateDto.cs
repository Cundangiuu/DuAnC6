namespace DuAnBanBanhKeo.Modal
{
    public class HoaDonNhapCreateDto
    {
        public string MaNCC { get; set; }
        public string TenNCC { get; set; }

        public decimal TongTien { get; set; }
        public int TrangThai { get; set; }
        public List<ChiTietHoaDonNhapDto> ChiTietHoaDonNhaps { get; set; }
    }
}
