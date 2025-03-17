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
        public async Task<ActionResult<List<DanhMuc>>> GetDanhMucSanPhams() // Updated return type to ActionResult<List<DanhMuc>>
        {
            var danhMucs = await _context.DanhMucs.ToListAsync();
            return Ok(danhMucs); // Directly return danhMucs list
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
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DanhMuc>> PostDanhMuc(DanhMuc danhMucSanPham)
        {
            if (DanhMucSanPhamExists(danhMucSanPham.MaDanhMuc))
            {
                return Conflict("Mã danh mục sản phẩm đã tồn tại.");
            }
            _context.DanhMucs.Add(danhMucSanPham);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (DanhMucSanPhamExists(danhMucSanPham.MaDanhMuc))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetDanhMucSanPham", new { maDanhMuc = danhMucSanPham.MaDanhMuc }, danhMucSanPham);
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

            _context.DanhMucs.Remove(danhMucSanPham);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DanhMucSanPhamExists(string maDanhMuc)
        {
            return _context.DanhMucs.Any(e => e.MaDanhMuc == maDanhMuc);
        }
    }
}