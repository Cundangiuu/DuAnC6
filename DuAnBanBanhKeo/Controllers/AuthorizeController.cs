using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DuAnBanBanhKeo.Data;
using DuAnBanBanhKeo.Modal;
using DuAnBanBanhKeo.Api.Modal;

namespace TestLab1_PK03529.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly JwtSettings jwtSettings;
        public AuthorizeController(ApplicationDbContext context, IOptions<JwtSettings> options)
        {
            _context = context;
            this.jwtSettings = options.Value;
        }
        [HttpPost("GenerateToken")]
        public async Task<IActionResult> GenerateToken([FromBody] TaiKhoanCredential taiKhoanCredential)
        {
            if (taiKhoanCredential == null || string.IsNullOrWhiteSpace(taiKhoanCredential.UserName) || string.IsNullOrWhiteSpace(taiKhoanCredential.Password))
            {
                return BadRequest("Tên người dùng hoặc mật khẩu không hợp lệ.");
            }

            var user = _context.TaiKhoans.SingleOrDefault(nv =>
                nv.TenTaiKhoan == taiKhoanCredential.UserName && nv.MatKhau == taiKhoanCredential.Password);

            if (user == null) return Unauthorized();

            // Kiểm tra giá trị SecurityKey
            if (string.IsNullOrEmpty(jwtSettings.SecurityKey))
            {
                return StatusCode(500, "SecurityKey chưa được cấu hình.");
            }

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.TenTaiKhoan),
                new Claim(ClaimTypes.Role, user.VaiTro),  // Lấy role từ user
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecurityKey));

            var token = new JwtSecurityToken(
                expires: DateTime.Now.AddMinutes(jwtSettings.DurationInMinutes), // Bỏ `Issuer` và `Audience`
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            });
        }

    }
}
