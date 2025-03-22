using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


namespace DuAnBanBanhKeo.Data.Entities
{
    public class DanhMuc
    {
        [Key]
        
        public string? MaDanhMuc { get; set; }
        
        public string? TenDanhMuc { get; set; } = string.Empty;
        
        public string? MoTa { get; set; }

        [JsonIgnore]
        public ICollection<SanPham> SanPhams { get; set; } = new List<SanPham>();
    }
}