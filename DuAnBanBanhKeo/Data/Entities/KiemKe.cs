using System.ComponentModel.DataAnnotations;

namespace DuAnBanBanhKeo.Data.Entities
{
    public class KiemKe
    {
        [Key]
        public string MaKiemKe { get; set; }
        public DateTime NgayKiemKe { get; set; } = DateTime.Now;
        public string MaNV { get; set; }
        public NhanVien NhanVien { get; set; }
        public string GhiChu { get; set; }
        public int TrangThai { get; set; } = 0; // 0: Chưa duyệt, 1: Đã duyệt
        public ICollection<ChiTietKiemKe> ChiTietKiemKes { get; set; } = new List<ChiTietKiemKe>();
    }
}