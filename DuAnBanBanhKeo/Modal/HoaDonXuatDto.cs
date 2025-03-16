namespace DuAnBanBanhKeo.Modal
{
    public class HoaDonXuatDto
    {
        public string MaHoaDonXuat { get; set; }
        public DateTime NgayXuat { get; set; }
        public string MaNV { get; set; }
        public string MaKH { get; set; }
        public decimal TongTien { get; set; }
        public int TrangThai { get; set; }

        public NhanVienDto NhanVien { get; set; } // Bao gồm dữ liệu liên quan nếu cần
        public KhachHangDto KhachHang { get; set; } // Bao gồm dữ liệu liên quan nếu cần

        public List<ChiTietHoaDonXuatDto> ChiTietHoaDonXuat { get; set; }
    }
}
