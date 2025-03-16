namespace DuAnBanBanhKeo.Modal
{
    public class HoaDonXuatCreateDto
    {
        public string MaKH { get; set; }

        public decimal TongTien { get; set; }

        public List<ChiTietHoaDonXuatDto> ChiTietHoaDonXuats { get; set; }
    }
}
