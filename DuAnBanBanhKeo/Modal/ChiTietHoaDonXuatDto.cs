namespace DuAnBanBanhKeo.Modal
{
    public class ChiTietHoaDonXuatDto
    {
        public string MaHoaDonXuat { get; set; }
        public string MaSP { get; set; }
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }

        public SanPhamDto SanPham { get; set; }
    }
}
