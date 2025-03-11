using System.ComponentModel.DataAnnotations;

namespace DuAnBanBanhKeo.Data.Entities
{
    public class PhieuNhap
    {
        [Key]
        public string MaPhieuNhap { get; set; }
        public DateTime NgayNhap { get; set; } = DateTime.Now;
        public decimal TongTien { get; set; }

        public int? MaNCC { get; set; }
        public NhaCungCap? NhaCungCap { get; set; }

        public ICollection<ChiTietPhieuNhap> ChiTietPhieuNhaps { get; set; } = new List<ChiTietPhieuNhap>();
    }
}
