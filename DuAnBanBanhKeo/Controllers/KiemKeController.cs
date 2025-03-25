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

        // GET: api/KiemKe (Phân trang, lọc theo ngày và trạng thái)
        [HttpGet]
        public async Task<ActionResult<object>> GetKiemKes(
            DateTime? ngayKiemKe = null,
            int? trangThai = null,
            int page = 1,
            int pageSize = 10)
        {
            var query = _context.KiemKes
                .Include(k => k.NhanVien)
                .Include(k => k.ChiTietKiemKes)
                    .ThenInclude(ct => ct.SanPham)
                .AsQueryable();

            if (ngayKiemKe.HasValue)
            {
                query = query.Where(k => k.NgayKiemKe.Date == ngayKiemKe.Value.Date);
            }

            if (trangThai.HasValue)
            {
                query = query.Where(k => k.TrangThai == trangThai.Value);
            }

            int totalItems = await query.CountAsync();
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var kiemKes = await query
                .OrderByDescending(k => k.NgayKiemKe)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var kiemKeViewModels = kiemKes.Select(kiemKe => new KiemKeViewModel
            {
                MaKiemKe = kiemKe.MaKiemKe,
                NgayKiemKe = kiemKe.NgayKiemKe,
                MaNV = kiemKe.MaNV,
                TenNhanVien = kiemKe.NhanVien?.HoTen, // Lấy tên nhân viên
                GhiChu = kiemKe.GhiChu,
                TrangThai = kiemKe.TrangThai,
                ChiTietKiemKes = kiemKe.ChiTietKiemKes.Select(ct => new ChiTietKiemKeViewModel
                {
                    MaSP = ct.MaSP,
                    TenSanPham = ct.SanPham.TenSP,
                    SoLuongTonKho = (int)ct.SanPham.SoLuongTon,
                    SoLuongThucTe = ct.SoLuongThucTe,
                    ChenhLechSoLuong = ct.ChenhLechSoLuong,
                    GhiChu = ct.GhiChu
                }).ToList()
            }).ToList();

            return Ok(new
            {
                Items = kiemKeViewModels,
                TotalItems = totalItems,
                TotalPages = totalPages,
                CurrentPage = page,
                PageSize = pageSize
            });
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

            var kiemKeViewModel = new KiemKeViewModel
            {
                MaKiemKe = kiemKe.MaKiemKe,
                NgayKiemKe = kiemKe.NgayKiemKe,
                MaNV = kiemKe.MaNV,
                TenNhanVien = kiemKe.NhanVien?.HoTen, // Lấy tên nhân viên
                GhiChu = kiemKe.GhiChu,
                TrangThai = kiemKe.TrangThai,
                ChiTietKiemKes = kiemKe.ChiTietKiemKes.Select(ct => new ChiTietKiemKeViewModel
                {
                    MaSP = ct.MaSP,
                    TenSanPham = ct.SanPham.TenSP,
                    SoLuongTonKho = (int)ct.SanPham.SoLuongTon,
                    SoLuongThucTe = ct.SoLuongThucTe,
                    ChenhLechSoLuong = ct.ChenhLechSoLuong,
                    GhiChu = ct.GhiChu
                }).ToList()
            };

            return kiemKeViewModel;
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
                NgayKiemKe = DateTime.Now,
                TrangThai = 0 // Mặc định là "Chưa duyệt"
            };

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
                    ChenhLechSoLuong = (int)(sanPham.SoLuongTon - chiTietInput.SoLuongThucTe),
                    GhiChu = chiTietInput.GhiChu
                };

                kiemKe.ChiTietKiemKes.Add(chiTietKiemKe);
            }

            _context.KiemKes.Add(kiemKe);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetKiemKe", new { id = kiemKe.MaKiemKe }, kiemKe);
        }

        // PATCH: api/KiemKe/5 (Chỉ điều chỉnh trạng thái)
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateTrangThai(string id, [FromBody] int trangThai)
        {
            var kiemKe = await _context.KiemKes.FindAsync(id);
            if (kiemKe == null)
            {
                return NotFound();
            }

            kiemKe.TrangThai = trangThai;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private async Task<string> GenerateMaKiemKeAsync()
        {
            string prefix = "KK";
            int nextId = 1;

            var lastKiemKe = await _context.KiemKes
                .OrderByDescending(k => k.MaKiemKe)
                .FirstOrDefaultAsync();

            if (lastKiemKe != null && lastKiemKe.MaKiemKe.StartsWith(prefix))
            {
                if (int.TryParse(lastKiemKe.MaKiemKe.Substring(prefix.Length), out int lastId))
                {
                    nextId = lastId + 1;
                }
            }

            return prefix + nextId.ToString("D3");
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