using DuAnBanBanhKeo.Data;
using DuAnBanBanhKeo.Modal;
using DuAnBanBanhKeo.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DuAnBanBanhKeo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizeController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly JwtSettings jwtSettings;
        private readonly IMemoryCache _cache;
        private readonly IEmailService _emailService;  

        public AuthorizeController(
            ApplicationDbContext context,
            IOptions<JwtSettings> options,
            IMemoryCache cache,
            IEmailService emailService)  
        {
            _context = context;
            this.jwtSettings = options.Value;
            _cache = cache;
            _emailService = emailService;  
        }

        [HttpPost("GenerateToken")]
        public async Task<IActionResult> GenerateToken([FromBody] TaiKhoanCredential taiKhoanCredential)
        {
            var user = await _context.TaiKhoans
                .Include(t => t.NhanVien)
                .SingleOrDefaultAsync(nv =>
                    nv.TenDangNhap == taiKhoanCredential.TenDangNhap &&
                    nv.MatKhau == taiKhoanCredential.MatKhau);

            if (user == null) return Unauthorized();

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.NhanVien.HoTen),
                new Claim(ClaimTypes.Role, user.NhanVien.VaiTro),
                new Claim("MaNV", user.MaNV),
                new Claim("HinhAnh", user.NhanVien.HinhAnh ?? ""),
                new Claim("TrangThaiTK", user.TrangThai.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecurityKey));

            var token = new JwtSecurityToken(
                expires: DateTime.Now.AddMinutes(jwtSettings.DurationInMinutes),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            });
        }

        [HttpPost("SendOtp")]
        public async Task<IActionResult> SendOtp([FromBody] SendOtpRequest request)
        {
            if (string.IsNullOrEmpty(request.Email))
            {
                return BadRequest(new { message = "Email không được để trống." });
            }

            var otpCode = GenerateOtpCode();
            var otpMessage = $"Mã OTP của bạn là: {otpCode}. Mã có hiệu lực trong 5 phút.";
            var otpModel = new OtpModel
            {
                Email = request.Email,
                OtpCode = otpCode,
                ExpiryTime = DateTime.UtcNow.AddMinutes(5)
            };

            _cache.Set(request.Email, otpModel, TimeSpan.FromMinutes(5));

            try
            {
                await _emailService.SendEmailAsync(request.Email, "Mã OTP đặt lại mật khẩu", otpMessage);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Có lỗi xảy ra khi gửi OTP qua email." });
            }

            return Ok(new { message = "Mã OTP đã được gửi đến email của bạn." });
        }

        private string GenerateOtpCode()
        {
            Random rand = new Random();
            return rand.Next(100000, 999999).ToString();
        }
    }
}
