using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace DuAnBanBanhKeo.Data.Entities
{
    public class DanhMuc
    {
        [Key]
        [MaxLength(10)] 
        public string MaDanhMuc { get; set; }
        [Required]
        [MaxLength(100)] 
        public string TenDanhMuc { get; set; } = string.Empty;
        [MaxLength(255)] 
        public string? MoTa { get; set; }
        [JsonIgnore]
        public ICollection<SanPham> SanPhams { get; set; } = new List<SanPham>();
    }
}