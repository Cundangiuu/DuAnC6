using DuAnBanBanhKeo.Data;
using DuAnBanBanhKeo.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using System;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;

namespace DuAnBanBanhKeo.Controllers // Đảm bảo namespace này khớp với project của bạn
{
    public class JwtSettings
    {
        public string SecurityKey { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
    }

    [ApiController]
    [Route("api/[controller]")]
    public class DangNhapController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DangNhapController> _logger;
        private readonly JwtSettings _jwtSettings;

        public DangNhapController(IConfiguration configuration, ApplicationDbContext context, ILogger<DangNhapController> logger, IOptions<JwtSettings> jwtSettings)
        {
            _configuration = configuration;
            _context = context;
            _logger = logger;
            _jwtSettings = jwtSettings.Value;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            try
            {
                _logger.LogInformation($"Yêu cầu đăng nhập cho người dùng: {model.TenDangNhap}");

                // Xác thực người dùng
                var taiKhoan = await Authenticate(model.TenDangNhap, model.MatKhau);

                if (taiKhoan == null)
                {
                    _logger.LogWarning($"Đăng nhập không thành công cho người dùng: {model.TenDangNhap}. Sai tên đăng nhập hoặc mật khẩu hoặc tài khoản bị khóa.");
                    return Unauthorized(new { message = "Tên đăng nhập hoặc mật khẩu không đúng." }); // Mặc định là sai tên đăng nhập hoặc mật khẩu
                }

                // Kiểm tra lại trạng thái (đề phòng trường hợp có thay đổi sau khi truy vấn)
                if (!taiKhoan.TrangThai)
                {
                    return StatusCode(403, new { message = "Tài khoản hiện đang bị khóa." }); // Trả về 403
                }

                // Tạo JWT token
                var token = GenerateJwtToken(taiKhoan);

                _logger.LogInformation($"Người dùng {model.TenDangNhap} đăng nhập thành công.  MaNV: {taiKhoan.MaNV}, VaiTro: {taiKhoan.NhanVien.VaiTro}");

                // Trả về thông tin người dùng cùng với token
                return Ok(new
                {
                    token = token,
                    maNV = taiKhoan.MaNV,
                    hoTen = taiKhoan.NhanVien.HoTen,
                    vaiTro = taiKhoan.NhanVien.VaiTro,
                    message = "Đăng nhập thành công."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi trong quá trình đăng nhập.");
                return StatusCode(500, new { message = $"Lỗi server: {ex.Message}" });
            }
        }

        private async Task<TaiKhoan> Authenticate(string tenDangNhap, string matKhau)
        {
            _logger.LogInformation($"Đang xác thực người dùng: {tenDangNhap}");

            try
            {
                var taiKhoan = await _context.TaiKhoans
                    .Include(tk => tk.NhanVien)
                    .FirstOrDefaultAsync(tk => tk.TenDangNhap == tenDangNhap && tk.MatKhau == matKhau); // So sánh trực tiếp mật khẩu

                if (taiKhoan == null)
                {
                    _logger.LogWarning($"Không tìm thấy tài khoản với tên đăng nhập hoặc mật khẩu không đúng: {tenDangNhap}");
                    return null;
                }

                // Kiểm tra trạng thái tài khoản
                if (!taiKhoan.TrangThai)
                {
                    _logger.LogWarning($"Tài khoản {tenDangNhap} đã bị khóa.");
                    return null;
                }

                _logger.LogInformation($"Xác thực thành công cho người dùng: {tenDangNhap}");
                return taiKhoan;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi trong quá trình xác thực người dùng: {tenDangNhap}");
                return null;
            }
        }
        private string GenerateJwtToken(TaiKhoan taiKhoan)
        {
            var key = Encoding.ASCII.GetBytes(_jwtSettings.SecurityKey);
            var issuer = _jwtSettings.Issuer;
            var audience = _jwtSettings.Audience;

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("MaNV", taiKhoan.MaNV),  // Thêm MaNV vào claim
                    new Claim(ClaimTypes.Role, taiKhoan.NhanVien.VaiTro), // Thêm VaiTro
                    new Claim(JwtRegisteredClaimNames.Sub, taiKhoan.TenDangNhap),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);

            _logger.LogInformation($"JWT được tạo thành công cho người dùng: {taiKhoan.TenDangNhap}");

            return jwtToken;
        }

        [HttpGet("test-auth")]
        [Authorize]
        public IActionResult TestAuth()
        {
            var maNV = User.FindFirstValue("MaNV"); // Lấy MaNV từ claim
            _logger.LogInformation($"Người dùng truy cập API test-auth. MaNV của bạn là: {maNV}");
            return Ok(new { message = $"Bạn đã được xác thực. MaNV của bạn là: {maNV}" });
        }
    }

    public class LoginRequest
    {
        [Required]
        public string TenDangNhap { get; set; }
        [Required]
        public string MatKhau { get; set; }
    }
}