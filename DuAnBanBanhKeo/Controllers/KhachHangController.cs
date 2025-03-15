using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DuAnBanBanhKeo.Data;
using DuAnBanBanhKeo.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DuAnBanBanhKeo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KhachHangController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public KhachHangController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<KhachHang>>> GetKhachHangs(
            [FromQuery] string? searchTerm = null,
            [FromQuery] string? sortBy = null,
            [FromQuery] string? sortOrder = "asc")
        {
            IQueryable<KhachHang> query = _context.KhachHangs;

            // Tìm kiếm
            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                query = query.Where(kh =>
                    kh.TenKH.ToLower().Contains(searchTerm) ||
                    (kh.Email != null && kh.Email.ToLower().Contains(searchTerm)) ||
                    (kh.DiaChi != null && kh.DiaChi.ToLower().Contains(searchTerm)) ||
                    (kh.SoDienThoai != null && kh.SoDienThoai.Contains(searchTerm)));
            }

            // Sắp xếp
            if (!string.IsNullOrEmpty(sortBy))
            {
                switch (sortBy.ToLower())
                {
                    case "tenkh":
                        query = sortOrder.ToLower() == "desc" ? query.OrderByDescending(kh => kh.TenKH) : query.OrderBy(kh => kh.TenKH);
                        break;
                    case "makh":
                        query = sortOrder.ToLower() == "desc" ? query.OrderByDescending(kh => kh.MaKH) : query.OrderBy(kh => kh.MaKH);
                        break;
                    case "email":
                        query = sortOrder.ToLower() == "desc" ? query.OrderByDescending(kh => kh.Email) : query.OrderBy(kh => kh.Email);
                        break;
                    default:
                        query = query.OrderBy(kh => kh.MaKH);
                        break;
                }
            }
            else
            {
                query = query.OrderBy(kh => kh.MaKH);
            }

            return await query.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<KhachHang>> GetKhachHang(string id)
        {
            var khachHang = await _context.KhachHangs.FindAsync(id);

            if (khachHang == null)
            {
                return NotFound($"Không tìm thấy khách hàng có mã '{id}'.");
            }
            return khachHang;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutKhachHang(string id, KhachHang khachHang)
        {
            if (id != khachHang.MaKH)
            {
                return BadRequest("Mã khách hàng trong URL không khớp với mã khách hàng trong dữ liệu.");
            }

            if (!ModelState.IsValid)  // Kiểm tra ModelState.IsValid
            {
                return BadRequest(ModelState);  // Trả về lỗi validation
            }

            _context.Entry(khachHang).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KhachHangExists(id))
                {
                    return NotFound($"Không tìm thấy khách hàng có mã '{id}'.");
                }
                return Conflict("Dữ liệu đã bị thay đổi bởi người khác. Vui lòng thử lại.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Lỗi khi cập nhật khách hàng: {ex}");
                return StatusCode(500, "Lỗi khi lưu thông tin khách hàng vào database.");
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<KhachHang>> PostKhachHang(KhachHang khachHang)
        {
            if (!ModelState.IsValid)  // Kiểm tra ModelState.IsValid
            {
                return BadRequest(ModelState);  // Trả về lỗi validation
            }

            _context.KhachHangs.Add(khachHang);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (KhachHangExists(khachHang.MaKH))
                {
                    return Conflict("Mã khách hàng đã tồn tại.");
                }
                Console.Error.WriteLine($"Lỗi khi tạo khách hàng (DbUpdateException): {ex.Message}");
                return StatusCode(500, "Lỗi khi lưu thông tin khách hàng vào database.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Lỗi không xác định khi tạo khách hàng: {ex}");
                return StatusCode(500, "Lỗi server. Vui lòng liên hệ quản trị viên.");
            }

            return CreatedAtAction("GetKhachHang", new { id = khachHang.MaKH }, khachHang);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteKhachHang(string id)
        {
            var khachHang = await _context.KhachHangs.FindAsync(id);

            if (khachHang == null)
            {
                return NotFound($"Không tìm thấy khách hàng có mã '{id}'.");
            }

            try
            {
                _context.KhachHangs.Remove(khachHang);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                Console.Error.WriteLine($"Lỗi khi xóa khách hàng (DbUpdateException): {ex.Message}");
                return StatusCode(500, "Lỗi khi xóa khách hàng khỏi database. Vui lòng thử lại sau.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Lỗi không xác định khi xóa khách hàng: {ex}");
                return StatusCode(500, "Lỗi server. Vui lòng liên hệ quản trị viên.");
            }

            return NoContent();
        }

        private bool KhachHangExists(string id)
        {
            return _context.KhachHangs.Any(e => e.MaKH == id);
        }
    }
}