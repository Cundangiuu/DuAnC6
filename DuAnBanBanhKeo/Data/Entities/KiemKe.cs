using System.ComponentModel.DataAnnotations;

namespace DuAnBanBanhKeo.Data.Entities
{
    public class KiemKe
    {
        [Key]
        public string MaKiemKe { get; set; }

        public DateTime NgayKiemKe { get; set; } = DateTime.Now;

        public string MaNV { get; set; }
        public NhanVien NhanVien { get; set; } // Nhân viên thực hiện kiểm kê
        public string GhiChu { get; set; }

        public ICollection<ChiTietKiemKe> ChiTietKiemKes { get; set; } = new List<ChiTietKiemKe>();
    }


}
