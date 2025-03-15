using AutoMapper;
using DuAnBanBanhKeo.Data.Entities;
using DuAnBanBanhKeo.Data;
using DuAnBanBanhKeo.Modal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace DuAnBanBanhKeo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HoaDonNhapController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public HoaDonNhapController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/HoaDonNhap
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HoaDonNhapDto>>> GetHoaDonNhaps()
        {
            var hoaDonNhaps = await _context.HoaDonNhaps
                .Include(hdn => hdn.ChiTietHoaDonNhaps)
                .ThenInclude(ct => ct.SanPham)
                .Include(hdn => hdn.NhanVien)
                .Include(hdn => hdn.NhaCungCap)
                .ToListAsync();

            return _mapper.Map<List<HoaDonNhapDto>>(hoaDonNhaps);
        }

        // GET: api/HoaDonNhap/5
        [HttpGet("{maHoaDonNhap}")]
        public async Task<ActionResult<HoaDonNhapDto>> GetHoaDonNhap(string maHoaDonNhap)
        {
            var hoaDonNhap = await _context.HoaDonNhaps
                .Include(hdn => hdn.ChiTietHoaDonNhaps)
                .ThenInclude(ct => ct.SanPham)
                .Include(hdn => hdn.NhanVien)
                .Include(hdn => hdn.NhaCungCap)
                .FirstOrDefaultAsync(hdn => hdn.MaHoaDonNhap == maHoaDonNhap);

            if (hoaDonNhap == null)
            {
                return NotFound();
            }

            return _mapper.Map<HoaDonNhapDto>(hoaDonNhap);
        }

        // PUT: api/HoaDonNhap/5
        [HttpPut("{maHoaDonNhap}")]
        public async Task<IActionResult> PutHoaDonNhap(string maHoaDonNhap, HoaDonNhapUpdateDto hoaDonNhapUpdateDto)
        {
            if (maHoaDonNhap != hoaDonNhapUpdateDto.MaHoaDonNhap)
            {
                return BadRequest();
            }

            var hoaDonNhap = await _context.HoaDonNhaps
                .Include(hdn => hdn.ChiTietHoaDonNhaps)
                .FirstOrDefaultAsync(hdn => hdn.MaHoaDonNhap == maHoaDonNhap);

            if (hoaDonNhap == null)
            {
                return NotFound();
            }

            _mapper.Map(hoaDonNhapUpdateDto, hoaDonNhap);

            // Validate trạng thái
            if (!IsValidTrangThai(hoaDonNhapUpdateDto.TrangThai))
            {
                return BadRequest("Trạng thái không hợp lệ. Chỉ chấp nhận: 0, 1, 2, 3.");
            }

            hoaDonNhap.TrangThai = hoaDonNhapUpdateDto.TrangThai; // Cập nhật trạng thái

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HoaDonNhapExists(maHoaDonNhap))
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

        // POST: api/HoaDonNhap
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<HoaDonNhapDto>> PostHoaDonNhap(HoaDonNhapCreateDto hoaDonNhapCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var maNVClaim = User.Claims.FirstOrDefault(c => c.Type == "MaNV");

            if (maNVClaim == null)
            {
                return BadRequest("Không tìm thấy thông tin nhân viên trong token.");
            }

            string maNV = maNVClaim.Value;

            if (string.IsNullOrEmpty(maNV))
            {
                return BadRequest("Mã nhân viên không hợp lệ.");
            }

            string maHoaDonNhap = GenerateMaHoaDonNhap();

            var hoaDonNhap = new HoaDonNhap
            {
                MaHoaDonNhap = maHoaDonNhap,
                NgayNhap = DateTime.Now,
                MaNV = maNV,
                MaNCC = hoaDonNhapCreateDto.MaNCC,
                TongTien = hoaDonNhapCreateDto.TongTien,
                TrangThai = 0 // Đặt trạng thái mặc định
            };

            _context.HoaDonNhaps.Add(hoaDonNhap);

            foreach (var chiTietDto in hoaDonNhapCreateDto.ChiTietHoaDonNhaps)
            {
                var sanPham = await _context.SanPhams.FindAsync(chiTietDto.MaSP);
                if (sanPham != null)
                {
                    sanPham.SoLuongTon += chiTietDto.SoLuong;
                }

                var chiTiet = new ChiTietHoaDonNhap
                {
                    MaHoaDonNhap = hoaDonNhap.MaHoaDonNhap,
                    MaSP = chiTietDto.MaSP,
                    SoLuong = chiTietDto.SoLuong,
                    DonGia = chiTietDto.DonGia
                };
                _context.ChiTietHoaDonNhaps.Add(chiTiet);
            }

            await _context.SaveChangesAsync();

            // Load related entities for the DTO
            await _context.Entry(hoaDonNhap).Reference(h => h.NhaCungCap).LoadAsync();
            await _context.Entry(hoaDonNhap).Reference(h => h.NhanVien).LoadAsync();
            foreach (var ct in hoaDonNhap.ChiTietHoaDonNhaps)
            {
                await _context.Entry(ct).Reference(c => c.SanPham).LoadAsync();
            }

            return CreatedAtAction("GetHoaDonNhap", new { maHoaDonNhap = hoaDonNhap.MaHoaDonNhap }, _mapper.Map<HoaDonNhapDto>(hoaDonNhap));
        }

        private string GenerateMaHoaDonNhap()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            string maHoaDonNhap;

            do
            {
                maHoaDonNhap = "HDN" + new string(Enumerable.Repeat(chars, 6)
                    .Select(s => s[random.Next(s.Length)]).ToArray());
            }
            while (_context.HoaDonNhaps.Any(hdn => hdn.MaHoaDonNhap == maHoaDonNhap));

            return maHoaDonNhap;
        }

        private bool HoaDonNhapExists(string maHoaDonNhap)
        {
            return _context.HoaDonNhaps.Any(e => e.MaHoaDonNhap == maHoaDonNhap);
        }

        [HttpGet("Products")]
        public async Task<ActionResult<IEnumerable<SanPhamDto>>> GetProducts()
        {
            var sanPhams = await _context.SanPhams.ToListAsync();
            return _mapper.Map<List<SanPhamDto>>(sanPhams);
        }

        [HttpGet("Suppliers")]
        public async Task<ActionResult<IEnumerable<NhaCungCapDto>>> GetSuppliers()
        {
            var nhaCungCaps = await _context.NhaCungCaps.ToListAsync();
            return _mapper.Map<List<NhaCungCapDto>>(nhaCungCaps);
        }

        // Hàm kiểm tra trạng thái hợp lệ
        private bool IsValidTrangThai(int trangThai)
        {
            return trangThai == 0 || trangThai == 1 || trangThai == 2 || trangThai == 3;
        }
    }
}