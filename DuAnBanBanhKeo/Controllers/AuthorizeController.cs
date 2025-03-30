using DuAnBanBanhKeo.Data;
using DuAnBanBanhKeo.Modal;
using DuAnBanBanhKeo.Utilities;
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
    public class AuthorizeController : ControllerBase
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
            var user = await _context.TaiKhoans
                .Include(t => t.NhanVien)
                .SingleOrDefaultAsync(nv => nv.TenDangNhap == taiKhoanCredential.TenDangNhap);

            if (user == null || !PasswordHelper.VerifyPassword(taiKhoanCredential.MatKhau, user.MatKhau))
            {
                return Unauthorized("Tên đăng nhập hoặc mật khẩu không chính xác.");
            }

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

    }
}