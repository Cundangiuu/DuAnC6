using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DuAnBanBanhKeo.Data.Entities
{
    public class SanPham
    {
        [Key]
        public string MaSanPham { get; set; } = null!;


        public string TenSanPham { get; set; } = null!;


        public decimal Gia { get; set; }


        public string? MoTa { get; set; }


        public string? MaNhaCungCap { get; set; }


        public int SoLuong { get; set; }


        public DateOnly? NgayThem { get; set; }



        public string? DonVi { get; set; }


        public DateOnly? Nsx { get; set; }


        public DateOnly? Hsd { get; set; }


        public int? TrangThai { get; set; }


        [NotMapped]
        public List<IFormFile>? UploadImages { get; set; }

        public virtual ICollection<AnhSanPham> AnhSanPhams { get; set; } = new List<AnhSanPham>();

        public virtual ICollection<ChiTietCombo> ChiTietCombos { get; set; } = new List<ChiTietCombo>();

        public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; } = new List<ChiTietDonHang>();

        public virtual ICollection<ChiTietGioHang> ChiTietGioHangs { get; set; } = new List<ChiTietGioHang>();

        public virtual ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; } = new List<ChiTietHoaDon>();


        public virtual NhaCungCap? MaNhaCungCapNavigation { get; set; }
    }
}
