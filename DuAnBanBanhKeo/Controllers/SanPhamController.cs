using DuAnBanBanhKeo.Data;
using DuAnBanBanhKeo.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DuAnBanBanhKeo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SanPhamController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SanPhamController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/SanPham
        [HttpGet]
        public async Task<ActionResult<object>> GetSanPhams( // Thay đổi kiểu trả về thành ActionResult<object>
            string? searchTerm,
            string? maDanhMucFilter, // Filter by Category
            bool? trangThaiFilter,     // Filter by Status
            string? sortBy,             // Sort by field (e.g., "TenSP", "GiaBan")
            string? sortOrder = "asc", // Sorting order "asc" or "desc"
            int page = 1,
            int pageSize = 10)
        {
            IQueryable<SanPham> query = _context.SanPhams
                .Include(sp => sp.NhaCungCap)
                .Include(sp => sp.DanhMuc);

            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                query = query.Where(sp =>
                    sp.TenSP.ToLower().Contains(searchTerm) ||
                    (sp.NhaCungCap != null && sp.NhaCungCap.TenNCC.ToLower().Contains(searchTerm)) ||
                    (sp.DanhMuc != null && sp.DanhMuc.TenDanhMuc.ToLower().Contains(searchTerm))
                );
            }

            // Filter by Category
            if (!string.IsNullOrEmpty(maDanhMucFilter))
            {
                query = query.Where(sp => sp.MaDanhMuc == maDanhMucFilter);
            }

            // Filter by Status
            if (trangThaiFilter.HasValue)
            {
                query = query.Where(sp => sp.TrangThai == trangThaiFilter.Value);
            }

            // Sorting
            if (!string.IsNullOrEmpty(sortBy))
            {
                if (sortBy.ToLower() == "tensp")
                {
                    query = sortOrder.ToLower() == "desc" ? query.OrderByDescending(sp => sp.TenSP) : query.OrderBy(sp => sp.TenSP);
                }
                // Add more sorting options if needed (e.g., by GiaNhap, GiaBan, etc.)
            }
            else
            {
                query = query.OrderBy(sp => sp.MaSP); // Default sorting
            }


            int totalItems = await query.CountAsync();
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var sanPhams = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Trả về object ẩn danh chứa Items là List<SanPham>
            return Ok(new
            {
                Items = sanPhams, // Items bây giờ là danh sách SanPham trực tiếp
                TotalItems = totalItems,
                TotalPages = totalPages,
                PageSize = pageSize,
                CurrentPage = page
            });
        }

        // GET: api/SanPham/MASP
        [HttpGet("{maSP}")]
        public async Task<ActionResult<SanPham>> GetSanPham(string maSP)
        {
            var sanPham = await _context.SanPhams.FindAsync(maSP);

            if (sanPham == null)
            {
                return NotFound();
            }

            return Ok(sanPham); // Trả về Ok(sanPham)
        }

        // POST: api/SanPham
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SanPham>> PostSanPham(SanPham sanPham)
        {
            // **Tự động tạo MaSP**
            string maSPMoi = await GenerateMaSP();
            sanPham.MaSP = maSPMoi;

            if (SanPhamExists(sanPham.MaSP)) // Check if generated MaSP already exists (unlikely, but good to check)
            {
                return Conflict("Mã sản phẩm đã tồn tại."); // Should not happen in normal cases
            }

            if (!ModelState.IsValid) // **KIỂM TRA MODEL STATE**
            {
                // **LOG MODEL STATE ERRORS**
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    System.Diagnostics.Debug.WriteLine($"Validation Error: {error.ErrorMessage}"); // In ra Output window trong VS
                }
                return BadRequest(ModelState); // Return BadRequest with validation errors
            }

            _context.SanPhams.Add(sanPham);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (SanPhamExists(sanPham.MaSP))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetSanPham", new { maSP = sanPham.MaSP }, sanPham); // Trả về sanPham trực tiếp
        }

        // DELETE: api/SanPham/MASP
        [HttpDelete("{maSP}")]
        public async Task<IActionResult> DeleteSanPham(string maSP)
        {
            var sanPham = await _context.SanPhams.FindAsync(maSP);
            if (sanPham == null)
            {
                return NotFound();
            }

            _context.SanPhams.Remove(sanPham);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SanPhamExists(string maSP)
        {
            return _context.SanPhams.Any(e => e.MaSP == maSP);
        }

        private async Task<string> GenerateMaSP()
        {
            string prefix = "SP";
            int startingNumber = 1;
            int maSPNumber;

            // Lấy MaSP cuối cùng hiện có trong database
            var lastSanPham = await _context.SanPhams
                .OrderByDescending(sp => sp.MaSP)
                .FirstOrDefaultAsync();

            if (lastSanPham == null)
            {
                maSPNumber = startingNumber; // Nếu không có sản phẩm nào, bắt đầu từ SP001
            }
            else
            {
                string lastMaSP = lastSanPham.MaSP;
                if (lastMaSP.StartsWith(prefix) && int.TryParse(lastMaSP.Substring(prefix.Length), out int lastNumber))
                {
                    maSPNumber = lastNumber + 1; // Tăng số cuối lên 1
                }
                else
                {
                    maSPNumber = startingNumber; // Nếu MaSP cuối không đúng định dạng, bắt đầu lại từ SP001 (hoặc xử lý khác tùy logic)
                }
            }

            // Định dạng số thành chuỗi có 3 chữ số (ví dụ: 1 -> "001", 10 -> "010")
            string formattedNumber = maSPNumber.ToString("D3"); // "D3" là format specifier cho 3 chữ số với số 0 đứng đầu
            return prefix + formattedNumber; // Kết hợp prefix và số đã format để tạo MaSP mới (ví dụ: "SP001")
        }
    }
}