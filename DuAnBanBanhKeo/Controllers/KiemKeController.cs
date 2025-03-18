using DuAnBanBanhKeo.Data;
using DuAnBanBanhKeo.Data.Entities;
using DuAnBanBanhKeo.Modal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DuAnBanBanhKeo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KiemKeController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public KiemKeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/KiemKe/5
        [HttpGet("{id}")]
        public async Task<ActionResult<KiemKeViewModel>> GetKiemKe(string id)
        {
            var kiemKe = await _context.KiemKes
                .Include(k => k.NhanVien)
                .Include(k => k.ChiTietKiemKes)
                    .ThenInclude(ct => ct.SanPham)
                .FirstOrDefaultAsync(k => k.MaKiemKe == id);

            if (kiemKe == null)
            {
                return NotFound();
            }

            // Chuyển đổi sang KiemKeViewModel
            var kiemKeViewModel = new KiemKeViewModel
            {
                MaKiemKe = kiemKe.MaKiemKe,
                NgayKiemKe = kiemKe.NgayKiemKe,
                MaNV = kiemKe.MaNV,
                GhiChu = kiemKe.GhiChu,
                ChiTietKiemKes = kiemKe.ChiTietKiemKes.Select(ct => new ChiTietKiemKeViewModel
                {
                    MaSP = ct.MaSP,
                    TenSanPham = ct.SanPham.TenSP,
                    SoLuongTonKho = ct.SanPham.SoLuongTon, // Lấy số lượng tồn kho
                    SoLuongThucTe = ct.SoLuongThucTe,
                    GhiChu = ct.GhiChu
                }).ToList()
            };

            return kiemKeViewModel;
        }

        // GET: api/KiemKe
        [HttpGet]
        public async Task<ActionResult<IEnumerable<KiemKeViewModel>>> GetKiemKes()
        {
            var kiemKes = await _context.KiemKes
                .Include(k => k.NhanVien)
                .Include(k => k.ChiTietKiemKes)
                    .ThenInclude(ct => ct.SanPham)
                .ToListAsync();

            var kiemKeViewModels = kiemKes.Select(kiemKe => new KiemKeViewModel
            {
                MaKiemKe = kiemKe.MaKiemKe,
                NgayKiemKe = kiemKe.NgayKiemKe,
                MaNV = kiemKe.MaNV,
                GhiChu = kiemKe.GhiChu,
                ChiTietKiemKes = kiemKe.ChiTietKiemKes.Select(ct => new ChiTietKiemKeViewModel
                {
                    MaSP = ct.MaSP,
                    TenSanPham = ct.SanPham.TenSP,
                    SoLuongTonKho = ct.SanPham.SoLuongTon, // Lấy số lượng tồn kho
                    SoLuongThucTe = ct.SoLuongThucTe,
                    GhiChu = ct.GhiChu
                }).ToList()
            }).ToList();

            return kiemKeViewModels;
        }


        // POST: api/KiemKe
        [HttpPost]
        public async Task<ActionResult<KiemKe>> PostKiemKe(KiemKeInputModel kiemKeInput)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string maKiemKe = await GenerateMaKiemKeAsync();

            var kiemKe = new KiemKe
            {
                MaKiemKe = maKiemKe,
                MaNV = kiemKeInput.MaNV,
                GhiChu = kiemKeInput.GhiChu,
                NgayKiemKe = DateTime.Now
            };

            // Thêm chi tiết kiểm kê
            foreach (var chiTietInput in kiemKeInput.ChiTietKiemKes)
            {
                var sanPham = await _context.SanPhams.FindAsync(chiTietInput.MaSP);
                if (sanPham == null)
                {
                    return BadRequest($"Không tìm thấy sản phẩm có mã {chiTietInput.MaSP}");
                }

                var chiTietKiemKe = new ChiTietKiemKe
                {
                    MaKiemKe = maKiemKe,
                    MaSP = chiTietInput.MaSP,
                    SoLuongThucTe = chiTietInput.SoLuongThucTe,
                    GhiChu = chiTietInput.GhiChu
                };

                kiemKe.ChiTietKiemKes.Add(chiTietKiemKe);

                // Cập nhật số lượng tồn kho
                sanPham.SoLuongTon = chiTietInput.SoLuongThucTe;
                _context.Update(sanPham);
            }

            _context.KiemKes.Add(kiemKe);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetKiemKe", new { id = kiemKe.MaKiemKe }, kiemKe);
        }
        // PUT: api/KiemKe/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutKiemKe(string id, KiemKe kiemKe)
        {
            if (id != kiemKe.MaKiemKe)
            {
                return BadRequest();
            }

            _context.Entry(kiemKe).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KiemKeExists(id))
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

        // DELETE: api/KiemKe/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteKiemKe(string id)
        {
            var kiemKe = await _context.KiemKes.FindAsync(id);
            if (kiemKe == null)
            {
                return NotFound();
            }

            // Xóa chi tiết kiểm kê liên quan
            var chiTietKiemKes = await _context.ChiTietKiemKes.Where(ct => ct.MaKiemKe == id).ToListAsync();
            _context.ChiTietKiemKes.RemoveRange(chiTietKiemKes);

            _context.KiemKes.Remove(kiemKe);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool KiemKeExists(string id)
        {
            return _context.KiemKes.Any(e => e.MaKiemKe == id);
        }

        private async Task<string> GenerateMaKiemKeAsync()
        {
            string prefix = "KK";
            int nextId = 1;

            var lastKiemKe = await _context.KiemKes.ToListAsync();
            if (lastKiemKe.Any())
            {
                var lastMaKiemKe = lastKiemKe.Max(k => k.MaKiemKe);
                if (!string.IsNullOrEmpty(lastMaKiemKe) && lastMaKiemKe.StartsWith(prefix))
                {
                    if (int.TryParse(lastMaKiemKe.Substring(prefix.Length), out int lastId))
                    {
                        nextId = lastId + 1;
                    }
                }
            }

            string newMaKiemKe = prefix + nextId.ToString("D3");
            return newMaKiemKe;
        }
    }

    // InputModel để nhận dữ liệu từ client
    public class KiemKeInputModel
    {
        public string MaNV { get; set; }
        public string GhiChu { get; set; }

        public List<ChiTietKiemKeInputModel> ChiTietKiemKes { get; set; } = new List<ChiTietKiemKeInputModel>();
    }

    public class ChiTietKiemKeInputModel
    {
        public string MaSP { get; set; }
        public int SoLuongThucTe { get; set; }
        public string GhiChu { get; set; }
    }
}