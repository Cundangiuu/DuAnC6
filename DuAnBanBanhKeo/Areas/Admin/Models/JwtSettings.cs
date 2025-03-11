namespace DuAnBanBanhKeo.Areas.Admin.Models
{
    public class JwtSettings
    {
        public string SecurityKey { get; set; }

        public int DurationInMinutes { get; set; }
    }
}
