using AutoMapper;
using DuAnBanBanhKeo.Data;
using DuAnBanBanhKeo.Data.Entities;
using DuAnBanBanhKeo.Modal;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using LoginRequest = DuAnBanBanhKeo.Modal.LoginRequest;

namespace DuAnBanBanhKeo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaiKhoanController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public TaiKhoanController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaiKhoanDto>>> GetTaiKhoans()
        {
            var taiKhoans = await _context.TaiKhoans
                .Include(tk => tk.NhanVien) // Giả sử bạn có navigation property "MaNVNavigation"
                .ToListAsync();

            var taiKhoanDtos = taiKhoans.Select(tk => new TaiKhoanDto
            {
                MaTK = tk.MaTK,
                TenDangNhap = tk.TenDangNhap,
                MaNV = tk.MaNV,
                HoTen = tk.NhanVien?.HoTen, // Sửa thành HoTen
                MatKhau = tk.MatKhau,
                VaiTro = tk.NhanVien.VaiTro,
                TrangThai = tk.TrangThai
            }).ToList();

            return taiKhoanDtos;
        }

        [HttpGet("{maTK}")]
        public async Task<ActionResult<TaiKhoanDto>> GetTaiKhoan(string maTK)
        {
            var taiKhoan = await _context.TaiKhoans
                .Include(tk => tk.NhanVien)
                .FirstOrDefaultAsync(tk => tk.MaTK == maTK);

            if (taiKhoan == null)
            {
                return NotFound();
            }

            var taiKhoanDto = new TaiKhoanDto
            {
                MaTK = taiKhoan.MaTK,
                TenDangNhap = taiKhoan.TenDangNhap,
                MaNV = taiKhoan.MaNV,
                HoTen = taiKhoan.NhanVien?.HoTen, // Sửa thành HoTen
                MatKhau = taiKhoan.MatKhau,
                VaiTro = taiKhoan.NhanVien.VaiTro,
                TrangThai = taiKhoan.TrangThai
            };

            return taiKhoanDto;
        }

        [HttpPut("{maTK}")]
        public async Task<IActionResult> PutTaiKhoan(string maTK, TaiKhoanUpdateDto taiKhoanUpdateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingTaiKhoan = await _context.TaiKhoans.FindAsync(maTK);

            if (existingTaiKhoan == null)
            {
                return NotFound();
            }

            existingTaiKhoan.TenDangNhap = taiKhoanUpdateDto.TenDangNhap;
            existingTaiKhoan.MaNV = taiKhoanUpdateDto.MaNV;

            // Cập nhật trạng thái nếu có
            if (taiKhoanUpdateDto.TrangThai.HasValue)
            {
                existingTaiKhoan.TrangThai = taiKhoanUpdateDto.TrangThai.Value;
            }

            if (!string.IsNullOrEmpty(taiKhoanUpdateDto.MatKhau))
            {
                string salt = GenerateSalt();
                string hashedPassword = HashPassword(taiKhoanUpdateDto.MatKhau, salt);
                existingTaiKhoan.MatKhau = hashedPassword + ":" + salt;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaiKhoanExists(maTK))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<TaiKhoan>> PostTaiKhoan(TaiKhoanCreateDto taiKhoanCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string salt = GenerateSalt();
            string hashedPassword = HashPassword(taiKhoanCreateDto.MatKhau, salt);

            // Tạo mã tài khoản tự động
            string maTK = GenerateMaTK();

            var taiKhoan = new TaiKhoan
            {
                MaTK = maTK,
                TenDangNhap = taiKhoanCreateDto.TenDangNhap,
                MatKhau = hashedPassword + ":" + salt,
                MaNV = taiKhoanCreateDto.MaNV,
                TrangThai = true 
            };

            _context.TaiKhoans.Add(taiKhoan);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTaiKhoan", new { maTK = taiKhoan.MaTK }, _mapper.Map<TaiKhoanDto>(taiKhoan));
        }

        [HttpDelete("{maTK}")]
        public async Task<IActionResult> DeleteTaiKhoan(string maTK)
        {
            var taiKhoan = await _context.TaiKhoans.FindAsync(maTK);
            if (taiKhoan == null)
            {
                return NotFound();
            }

            // Thay vì xóa, cập nhật trạng thái thành false (ngưng hoạt động)
            taiKhoan.TrangThai = false;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TaiKhoanExists(string maTK)
        {
            return _context.TaiKhoans.Any(e => e.MaTK == maTK);
        }

        private string GenerateSalt()
        {
            byte[] salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return Convert.ToBase64String(salt);
        }

        private string HashPassword(string password, string salt)
        {
            string passwordWithSalt = password + salt;
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(passwordWithSalt));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private string GenerateMaTK()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            string maTK;

            do
            {
                maTK = "TK" + new string(Enumerable.Repeat(chars, 6)
                    .Select(s => s[random.Next(s.Length)]).ToArray()); // 6 ký tự ngẫu nhiên sau "TK"
            }
            while (_context.TaiKhoans.Any(tk => tk.MaTK == maTK)); // Đảm bảo không trùng lặp

            return maTK;
        }

        [HttpPost("Login")]
        public async Task<ActionResult<object>> Login(LoginRequest loginRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var taiKhoan = await _context.TaiKhoans
                .Include(tk => tk.NhanVien)
                .FirstOrDefaultAsync(tk => tk.TenDangNhap == loginRequest.TenDangNhap && tk.TrangThai == true); // Kiểm tra cả TrangThai

            if (taiKhoan == null)
            {
                return Unauthorized("Tên đăng nhập không tồn tại hoặc tài khoản đã bị vô hiệu hóa.");
            }

            // Xác thực mật khẩu
            string[] parts = taiKhoan.MatKhau.Split(':');
            string hashedPassword = parts[0];
            string salt = parts[1];

            string hashedPasswordAttempt = HashPassword(loginRequest.MatKhau, salt);

            if (hashedPassword != hashedPasswordAttempt)
            {
                return Unauthorized("Mật khẩu không đúng.");
            }

            // Tạo đối tượng trả về chứa thông tin tài khoản và nhân viên
            var loginResponse = new
            {
                taiKhoan.MaTK,
                taiKhoan.TenDangNhap,
                taiKhoan.MaNV,
                HoTenNhanVien = taiKhoan.NhanVien?.HoTen,
                ChucVu = taiKhoan.NhanVien?.VaiTro // Thêm thông tin chức vụ nếu cần
            };

            return Ok(loginResponse);
        }
    }
}