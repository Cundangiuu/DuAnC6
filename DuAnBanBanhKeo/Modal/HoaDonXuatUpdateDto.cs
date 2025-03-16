using System.ComponentModel.DataAnnotations;

namespace DuAnBanBanhKeo.Modal
{
    public class HoaDonXuatUpdateDto
    {
        public string MaHoaDonXuat { get; set; }

        public decimal TongTien { get; set; }

        public int TrangThai { get; set; }

        public List<ChiTietHoaDonXuatDto> ChiTietHoaDonXuats { get; set; }
    }
}
    
