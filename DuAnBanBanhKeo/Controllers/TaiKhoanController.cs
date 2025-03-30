using AutoMapper;
using DuAnBanBanhKeo.Data;
using DuAnBanBanhKeo.Data.Entities;
using DuAnBanBanhKeo.Modal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BCrypt.Net;

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
                .Include(tk => tk.NhanVien)
                .ToListAsync();

            var taiKhoanDtos = _mapper.Map<List<TaiKhoanDto>>(taiKhoans);
            return Ok(taiKhoanDtos);
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

            var taiKhoanDto = _mapper.Map<TaiKhoanDto>(taiKhoan);
            return Ok(taiKhoanDto);
        }

        [HttpPut("{maTK}")]
        public async Task<IActionResult> PutTaiKhoan(string maTK, TaiKhoanUpdateDto taiKhoanUpdateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingTaiKhoan = await _context.TaiKhoans
                .Include(tk => tk.NhanVien)
                .FirstOrDefaultAsync(tk => tk.MaTK == maTK);

            if (existingTaiKhoan == null)
            {
                return NotFound();
            }

            // Cập nhật các trường cơ bản
            existingTaiKhoan.TenDangNhap = taiKhoanUpdateDto.TenDangNhap ?? existingTaiKhoan.TenDangNhap;
            existingTaiKhoan.MaNV = taiKhoanUpdateDto.MaNV ?? existingTaiKhoan.MaNV;

            // Cập nhật trạng thái nếu có giá trị
            if (taiKhoanUpdateDto.TrangThai.HasValue)
            {
                existingTaiKhoan.TrangThai = taiKhoanUpdateDto.TrangThai.Value;
            }

            // Cập nhật mật khẩu nếu được cung cấp
            if (!string.IsNullOrEmpty(taiKhoanUpdateDto.MatKhau))
            {
                existingTaiKhoan.MatKhau = BCrypt.Net.BCrypt.HashPassword(taiKhoanUpdateDto.MatKhau);
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
                throw;
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<TaiKhoanDto>> PostTaiKhoan(TaiKhoanCreateDto taiKhoanCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var taiKhoan = new TaiKhoan
            {
                MaTK = GenerateMaTK(),
                TenDangNhap = taiKhoanCreateDto.TenDangNhap,
                MatKhau = BCrypt.Net.BCrypt.HashPassword(taiKhoanCreateDto.MatKhau), // Hash mật khẩu bằng BCrypt
                MaNV = taiKhoanCreateDto.MaNV,
                TrangThai = true
            };

            _context.TaiKhoans.Add(taiKhoan);
            await _context.SaveChangesAsync();

            var taiKhoanDto = _mapper.Map<TaiKhoanDto>(taiKhoan);
            return CreatedAtAction(nameof(GetTaiKhoan), new { maTK = taiKhoan.MaTK }, taiKhoanDto);
        }

        [HttpDelete("{maTK}")]
        public async Task<IActionResult> DeleteTaiKhoan(string maTK)
        {
            var taiKhoan = await _context.TaiKhoans.FindAsync(maTK);
            if (taiKhoan == null)
            {
                return NotFound();
            }

            // Soft delete: cập nhật trạng thái thay vì xóa
            taiKhoan.TrangThai = false;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("Login")]
        public async Task<ActionResult<object>> Login([FromBody] LoginRequest loginRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var taiKhoan = await _context.TaiKhoans
                .Include(tk => tk.NhanVien)
                .FirstOrDefaultAsync(tk => tk.TenDangNhap == loginRequest.TenDangNhap);

            if (taiKhoan == null || !taiKhoan.TrangThai)
            {
                return Unauthorized("Tên đăng nhập không tồn tại hoặc tài khoản đã bị vô hiệu hóa.");
            }

            // Kiểm tra mật khẩu bằng BCrypt
            if (!BCrypt.Net.BCrypt.Verify(loginRequest.MatKhau, taiKhoan.MatKhau))
            {
                return Unauthorized("Mật khẩu không đúng.");
            }

            var loginResponse = new
            {
                taiKhoan.MaTK,
                taiKhoan.TenDangNhap,
                taiKhoan.MaNV,
                HoTenNhanVien = taiKhoan.NhanVien?.HoTen,
                VaiTro = taiKhoan.NhanVien?.VaiTro
            };

            return Ok(loginResponse);
        }

        private bool TaiKhoanExists(string maTK)
        {
            return _context.TaiKhoans.Any(e => e.MaTK == maTK);
        }

        private string GenerateMaTK()
        {
            const string chars = "0123456789";
            var random = new Random();
            string maTK;

            do
            {
                maTK = "TK" + new string(Enumerable.Repeat(chars, 3)
                    .Select(s => s[random.Next(s.Length)]).ToArray());
            } while (_context.TaiKhoans.Any(tk => tk.MaTK == maTK));

            return maTK;
        }
    }
}