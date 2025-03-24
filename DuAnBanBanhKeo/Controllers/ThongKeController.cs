using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DuAnBanBanhKeo.Data;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace DuAnBanBanhKeo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThongKeController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ThongKeController> _logger;

        public ThongKeController(ApplicationDbContext context, ILogger<ThongKeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // ---------------------- Thống kê Sản phẩm ----------------------

        [HttpGet("SanPham/TongSoLuong")]
        public async Task<ActionResult<int>> GetTongSoLuongSanPham()
        {
            try
            {
                var totalProducts = await _context.SanPhams.CountAsync();
                return Ok(totalProducts);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Lỗi khi tính tổng số lượng sản phẩm: {ex.Message}");
                return StatusCode(500, "Lỗi khi tính tổng số lượng sản phẩm");
            }
        }

        [HttpGet("SanPham/TongGiaTriTonKho")]
        public async Task<ActionResult<decimal>> GetTongGiaTriTonKho()
        {
            try
            {
                var totalInventoryValue = await _context.SanPhams
                    .SumAsync(sp => sp.GiaBan * sp.SoLuongTon);
                return Ok(totalInventoryValue);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Lỗi khi tính tổng giá trị tồn kho: {ex.Message}");
                return StatusCode(500, "Lỗi khi tính tổng giá trị tồn kho");
            }
        }

        [HttpGet("SanPham/TongSoLuongTon")]
        public async Task<ActionResult<int>> GetTongSoLuongTon()
        {
            try
            {
                var tongSoLuongTon = await _context.SanPhams.SumAsync(sp => sp.SoLuongTon);
                return Ok(tongSoLuongTon);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Lỗi khi tính tổng số lượng tồn: {ex.Message}");
                return StatusCode(500, "Lỗi khi tính tổng số lượng tồn");
            }
        }

        [HttpGet("SanPham/SapHetHang")]
        public async Task<ActionResult<object>> GetSanPhamSapHetHang(int nguong = 10)
        {
            try
            {
                var sanPhamSapHetHang = await _context.SanPhams
                    .Where(sp => sp.SoLuongTon <= nguong)
                    .Select(sp => new { sp.MaSP, sp.TenSP, sp.SoLuongTon })
                    .ToListAsync();

                if (sanPhamSapHetHang == null || !sanPhamSapHetHang.Any())
                {
                    return NotFound("Không có sản phẩm nào sắp hết hàng");
                }

                return Ok(sanPhamSapHetHang);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Lỗi khi tìm sản phẩm sắp hết hàng: {ex.Message}");
                return StatusCode(500, "Lỗi khi tìm sản phẩm sắp hết hàng");
            }
        }

        [HttpGet("SanPham/HetHang")]
        public async Task<ActionResult<object>> GetSanPhamHetHang()
        {
            try
            {
                var sanPhamHetHang = await _context.SanPhams
                    .Where(sp => sp.SoLuongTon == 0)
                    .Select(sp => new { sp.MaSP, sp.TenSP })
                    .ToListAsync();

                return Ok(sanPhamHetHang);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Lỗi khi tìm sản phẩm hết hàng: {ex.Message}");
                return StatusCode(500, "Lỗi khi tìm sản phẩm hết hàng");
            }
        }

        [HttpGet("SanPham/NhapNhieuNhat")]
        public async Task<ActionResult<object>> GetSanPhamNhapNhieuNhat()
        {
            try
            {
                var sanPhamNhapNhieu = await _context.ChiTietHoaDonNhaps
                    .GroupBy(ct => ct.MaSP)
                    .OrderByDescending(g => g.Sum(ct => ct.SoLuong))
                    .Select(g => new {
                        MaSP = g.Key,
                        TongSoLuongNhap = g.Sum(ct => ct.SoLuong)
                    })
                    .FirstOrDefaultAsync();

                if (sanPhamNhapNhieu == null)
                {
                    return NotFound("Không có sản phẩm nào");
                }

                var tenSanPham = await _context.SanPhams
                    .Where(sp => sp.MaSP == sanPhamNhapNhieu.MaSP)
                    .Select(sp => sp.TenSP)
                    .FirstOrDefaultAsync();

                return Ok(new
                {
                    MaSP = sanPhamNhapNhieu.MaSP,
                    TenSP = tenSanPham,
                    TongSoLuongNhap = sanPhamNhapNhieu.TongSoLuongNhap
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Lỗi khi tìm sản phẩm nhập nhiều nhất: {ex.Message}");
                return StatusCode(500, "Lỗi khi tìm sản phẩm nhập nhiều nhất");
            }
        }

        [HttpGet("SanPham/NhapItNhat")]
        public async Task<ActionResult<object>> GetSanPhamNhapItNhat()
        {
            try
            {
                var sanPhamNhapIt = await _context.ChiTietHoaDonNhaps
                    .GroupBy(ct => ct.MaSP)
                    .OrderBy(g => g.Sum(ct => ct.SoLuong))
                    .Select(g => new {
                        MaSP = g.Key,
                        TongSoLuongNhap = g.Sum(ct => ct.SoLuong)
                    })
                    .FirstOrDefaultAsync();

                if (sanPhamNhapIt == null)
                {
                    return NotFound("Không có sản phẩm nào");
                }

                var tenSanPham = await _context.SanPhams
                    .Where(sp => sp.MaSP == sanPhamNhapIt.MaSP)
                    .Select(sp => sp.TenSP)
                    .FirstOrDefaultAsync();

                return Ok(new
                {
                    MaSP = sanPhamNhapIt.MaSP,
                    TenSP = tenSanPham,
                    TongSoLuongNhap = sanPhamNhapIt.TongSoLuongNhap
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Lỗi khi tìm sản phẩm nhập ít nhất: {ex.Message}");
                return StatusCode(500, "Lỗi khi tìm sản phẩm nhập ít nhất");
            }
        }

        [HttpGet("SanPham/PhanLoaiSanPham")]
        public async Task<ActionResult<object>> GetPhanLoaiSanPham()
        {
            try
            {
                var phanLoai = await _context.SanPhams
                    .GroupBy(sp => sp.MaDanhMuc)
                    .Select(g => new
                    {
                        MaLoai = g.Key,
                        TenLoai = _context.DanhMucs.FirstOrDefault(l => l.MaDanhMuc == g.Key).TenDanhMuc, // Lấy tên loại
                        SoLuong = g.Count()
                    })
                    .ToListAsync();

                return Ok(phanLoai);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Lỗi khi thống kê phân loại sản phẩm: {ex.Message}");
                return StatusCode(500, "Lỗi khi thống kê phân loại sản phẩm");
            }
        }


        // ---------------------- Thống kê Nhà cung cấp ----------------------

        [HttpGet("NhaCungCap/TongSoLuong")]
        public async Task<ActionResult<int>> GetTongSoLuongNhaCungCap()
        {
            try
            {
                var totalSuppliers = await _context.NhaCungCaps.CountAsync();
                return Ok(totalSuppliers);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Lỗi khi tính tổng số lượng nhà cung cấp: {ex.Message}");
                return StatusCode(500, "Lỗi khi tính tổng số lượng nhà cung cấp");
            }
        }

        [HttpGet("NhaCungCap/SoLuongSanPhamCungCap/{maNCC}")]
        public async Task<ActionResult<int>> GetSoLuongSanPhamCungCap(string maNCC)
        {
            try
            {
                // Logic này có thể không chính xác. Bạn nên xem xét lại cách tính số lượng sản phẩm cung cấp.
                // Đoạn code hiện tại tính tổng số lượng trong ChiTietPhieuNhaps, nhưng không đảm bảo rằng
                // mỗi sản phẩm là duy nhất.

                //Ví dụ sửa đổi (cần điều chỉnh theo logic nghiệp vụ của bạn):
                var soLuongSanPham = await _context.ChiTietPhieuNhaps
                    .Where(ct => ct.PhieuNhap.MaNCC == int.Parse(maNCC))
                    .Select(ct => ct.MaSP)
                    .Distinct() // Đảm bảo mỗi sản phẩm chỉ được đếm một lần
                    .CountAsync();

                return Ok(soLuongSanPham);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Lỗi khi tính số lượng sản phẩm cung cấp bởi nhà cung cấp {maNCC}: {ex.Message}");
                return StatusCode(500, "Lỗi khi tính số lượng sản phẩm cung cấp bởi nhà cung cấp");
            }
        }

        [HttpGet("NhaCungCap/TongGiaTriNhapHang/{maNCC}")]
        public async Task<ActionResult<decimal>> GetTongGiaTriNhapHang(string maNCC)
        {
            try
            {
                var tongGiaTriNhapHang = await _context.HoaDonNhaps
                    .Where(hd => hd.MaNCC == maNCC) // Sửa ở đây
                    .Join(
                        _context.ChiTietHoaDonNhaps,
                        hd => hd.MaHoaDonNhap,
                        ct => ct.MaHoaDonNhap,
                        (hd, ct) => new { hd, ct }
                    )
                    .SumAsync(s => s.ct.SoLuong * s.ct.DonGia);

                return Ok(tongGiaTriNhapHang);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Lỗi khi tính tổng giá trị nhập hàng từ nhà cung cấp {maNCC}: {ex.Message}");
                return StatusCode(500, "Lỗi khi tính tổng giá trị nhập hàng từ nhà cung cấp");
            }
        }


        //Thống kê Hóa đơn Nhập
        [HttpGet("HoaDonNhap/TongSoLuongHoaDonNhap")]
        public async Task<ActionResult<int>> GetTongSoLuongHoaDonNhap()
        {
            try
            {
                var totalHoaDonNhap = await _context.HoaDonNhaps.CountAsync();
                return Ok(totalHoaDonNhap);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Lỗi khi tính tổng số lượng hóa đơn nhập: {ex.Message}");
                return StatusCode(500, "Lỗi khi tính tổng số lượng hóa đơn nhập");
            }
        }

        [HttpGet("HoaDonNhap/GiaTriNhapTheoThang/{thang}")]
        public async Task<ActionResult<decimal>> GetGiaTriNhapTheoThang(int thang)
        {
            try
            {
                var giaTriNhap = await _context.HoaDonNhaps
                    .Where(hd => hd.NgayNhap.Month == thang)
                    .SumAsync(hd => hd.TongTien);

                return Ok(giaTriNhap);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Lỗi khi tính giá trị nhập theo tháng: {ex.Message}");
                return StatusCode(500, "Lỗi khi tính giá trị nhập theo tháng");
            }
        }
        //Thống kê Hóa đơn Xuất
        [HttpGet("HoaDonXuat/TongSoLuongHoaDonXuat")]
        public async Task<ActionResult<int>> GetTongSoLuongHoaDonXuat()
        {
            try
            {
                var totalHoaDonXuat = await _context.HoaDonXuats.CountAsync();
                return Ok(totalHoaDonXuat);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Lỗi khi tính tổng số lượng hóa đơn xuất: {ex.Message}");
                return StatusCode(500, "Lỗi khi tính tổng số lượng hóa đơn xuất");
            }
        }

        [HttpGet("HoaDonXuat/GiaTriXuatTheoThang/{thang}")]
        public async Task<ActionResult<decimal>> GetGiaTriXuatTheoThang(int thang)
        {
            try
            {
                var giaTriXuat = await _context.HoaDonXuats
                    .Where(hdx => hdx.NgayXuat.Month == thang)
                    .SumAsync(hdx => hdx.TongTien);

                return Ok(giaTriXuat);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Lỗi khi tính giá trị xuất theo tháng: {ex.Message}");
                return StatusCode(500, "Lỗi khi tính giá trị xuất theo tháng");
            }
        }

    }
}