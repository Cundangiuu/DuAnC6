using DuAnBanBanhKeo.Data;
using DuAnBanBanhKeo.Modal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DuAnBanBanhKeo.Controllers
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
            if (taiKhoanCredential == null || string.IsNullOrWhiteSpace(taiKhoanCredential.TenDangNhap) || string.IsNullOrWhiteSpace(taiKhoanCredential.MatKhau))
            {
                return BadRequest("Tên người dùng hoặc mật khẩu không hợp lệ.");
            }

            var user = _context.TaiKhoans
             .Include(t => t.NhanVien) // Include the related NhanVien entity
             .SingleOrDefault(nv =>
                 nv.TenDangNhap == taiKhoanCredential.TenDangNhap &&
                 nv.MatKhau == taiKhoanCredential.MatKhau);

            if (user == null) return Unauthorized();

            // Kiểm tra giá trị SecurityKey
            if (string.IsNullOrEmpty(jwtSettings.SecurityKey))
            {
                return StatusCode(500, "SecurityKey chưa được cấu hình.");
            }

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.TenDangNhap),
                new Claim(ClaimTypes.Role, user.NhanVien.VaiTro), 
                new Claim("MaNV", user.MaNV),
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

    }
}
