namespace DuAnBanBanhKeo.Modal
{
    public class HoaDonXuatCreateDto
    {
        public string MaNV { get; set; }
        public string MaKH { get; set; }

        public decimal TongTien { get; set; }
        //public string HoTen { get; set; }

        public List<ChiTietHoaDonXuatDto> ChiTietHoaDonXuats { get; set; }
    }
}
