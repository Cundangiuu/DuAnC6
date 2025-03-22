using DuAnBanBanhKeo.Data;
using DuAnBanBanhKeo.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DuAnBanBanhKeo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DanhMucController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DanhMucController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/DanhMucSanPham
        [HttpGet]
        public async Task<ActionResult<List<DanhMuc>>> GetDanhMucSanPhams( 
            int page = 1,
            int pageSize = 10,
            string? searchTerm = null,
            string? sortBy = null)
        {
            IQueryable<DanhMuc> query = _context.DanhMucs;

            // Tìm kiếm
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(dm => dm.TenDanhMuc.Contains(searchTerm));
            }

            // Sắp xếp
            if (sortBy == "az")
            {
                query = query.OrderBy(dm => dm.TenDanhMuc);
            }
            else if (sortBy == "za")
            {
                query = query.OrderByDescending(dm => dm.TenDanhMuc);
            }

            int totalCount = await query.CountAsync(); // Tổng số danh mục sau khi lọc

            // Phân trang
            var danhMucs = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(danhMucs); // Trả về cả danh sách và tổng số lượng
        }

        // GET: api/DanhMucSanPham/MADM
        [HttpGet("{maDanhMuc}")]
        public async Task<ActionResult<DanhMuc>> GetDanhMuc(string maDanhMuc)
        {
            var danhMucSanPham = await _context.DanhMucs.FindAsync(maDanhMuc);

            if (danhMucSanPham == null)
            {
                return NotFound();
            }

            return Ok(danhMucSanPham);
        }

        // POST: api/DanhMucSanPham
        [HttpPost]
        public async Task<ActionResult<DanhMuc>> PostDanhMuc(DanhMuc danhMucSanPham)
        {
            // Kiểm tra ModelState trước khi thực hiện bất kỳ thao tác nào
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Trả về BadRequest kèm theo lỗi validation chi tiết
            }

            // Auto-generate MaDanhMuc
            string latestMaDanhMuc = await _context.DanhMucs
                .OrderByDescending(dm => dm.MaDanhMuc)
                .Select(dm => dm.MaDanhMuc)
                .FirstOrDefaultAsync();

            int nextDanhMucNumber = 1;
            if (!string.IsNullOrEmpty(latestMaDanhMuc) && latestMaDanhMuc.StartsWith("DM"))
            {
                if (int.TryParse(latestMaDanhMuc.Substring(2), out int lastNumber))
                {
                    nextDanhMucNumber = lastNumber + 1;
                }
            }

            danhMucSanPham.MaDanhMuc = $"DM{nextDanhMucNumber:D3}"; // Format as DM001, DM010, DM100

            _context.DanhMucs.Add(danhMucSanPham);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw; // Re-throw exception, để middleware xử lý lỗi chung
            }

            return CreatedAtAction("GetDanhMuc", new { maDanhMuc = danhMucSanPham.MaDanhMuc }, danhMucSanPham);
        }

        // PUT: api/DanhMuc/MADM
        [HttpPut("{maDanhMuc}")]
        public async Task<IActionResult> PutDanhMuc(string maDanhMuc, DanhMuc danhMucUpdate) // Nhận danhMucUpdate từ body
        {
            if (maDanhMuc != danhMucUpdate.MaDanhMuc)
            {
                return BadRequest("Mã danh mục không khớp.");
            }

            var danhMuc = await _context.DanhMucs.FindAsync(maDanhMuc);
            if (danhMuc == null)
            {
                return NotFound();
            }

            danhMuc.TenDanhMuc = danhMucUpdate.TenDanhMuc; // Cập nhật các thuộc tính cần thiết
            danhMuc.MoTa = danhMucUpdate.MoTa;

            _context.Entry(danhMuc).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DanhMucSanPhamExists(maDanhMuc))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent(); // Trả về NoContent khi thành công
        }

        // DELETE: api/DanhMucSanPham/MADM
        [HttpDelete("{maDanhMuc}")]
        public async Task<IActionResult> DeleteDanhMucSanPham(string maDanhMuc)
        {
            var danhMucSanPham = await _context.DanhMucs.FindAsync(maDanhMuc);
            if (danhMucSanPham == null)
            {
                return NotFound();
            }

            // **Xóa các sản phẩm liên quan trước khi xóa danh mục**
            var sanPhamsLienQuan = await _context.SanPhams
                                                .Where(sp => sp.MaDanhMuc == maDanhMuc)
                                                .ToListAsync();

            if (sanPhamsLienQuan.Any())
            {
                _context.SanPhams.RemoveRange(sanPhamsLienQuan); // Xóa hàng loạt sản phẩm liên quan
            }

            _context.DanhMucs.Remove(danhMucSanPham); // Sau đó mới xóa danh mục

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                // Xử lý lỗi nếu cần, ví dụ log lỗi
                return StatusCode(500, "Lỗi khi xóa danh mục và sản phẩm liên quan.");
            }

            return NoContent();
        }

        private bool DanhMucSanPhamExists(string maDanhMuc)
        {
            return _context.DanhMucs.Any(e => e.MaDanhMuc == maDanhMuc);
        }
    }
}