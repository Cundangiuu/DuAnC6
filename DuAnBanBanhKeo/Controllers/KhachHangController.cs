using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DuAnBanBanhKeo.Data;
using DuAnBanBanhKeo.Data.Entities;
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
                    kh.Email.ToLower().Contains(searchTerm) ||
                    kh.DiaChi.ToLower().Contains(searchTerm) ||
                    kh.SoDienThoai != null && kh.SoDienThoai.Contains(searchTerm));
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
                    // Thêm các trường sắp xếp khác nếu cần
                    default:
                        // Sắp xếp mặc định nếu không có trường nào được chỉ định
                        query = query.OrderBy(kh => kh.MaKH); // Sắp xếp mặc định theo MaKH
                        break;
                }
            }
            else
            {
                // Sắp xếp mặc định nếu không có sortBy được cung cấp
                query = query.OrderBy(kh => kh.MaKH); // Sắp xếp mặc định theo MaKH
            }

            return await query.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<KhachHang>> GetKhachHang(string id)
        {
            var khachHang = await _context.KhachHangs.FindAsync(id);

            if (khachHang == null)
            {
                return NotFound();
            }
            return khachHang;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutKhachHang(string id, KhachHang khachHang)
        {
            if (id != khachHang.MaKH)
            {
                return BadRequest();
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
        public async Task<ActionResult<KhachHang>> PostKhachHang(KhachHang khachHang)
        {
            _context.KhachHangs.Add(khachHang);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (KhachHangExists(khachHang.MaKH))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }
            return CreatedAtAction("GetKhachHang", new { id = khachHang.MaKH }, khachHang);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteKhachHang(string id)
        {
            var khachHang = await _context.KhachHangs.FindAsync(id);
            if (khachHang == null)
            {
                return NotFound();
            }
            _context.KhachHangs.Remove(khachHang);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool KhachHangExists(string id)
        {
            return _context.KhachHangs.Any(e => e.MaKH == id);
        }
    }
}