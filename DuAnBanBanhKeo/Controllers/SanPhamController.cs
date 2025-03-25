using DuAnBanBanhKeo.Data;
using DuAnBanBanhKeo.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using DuAnBanBanhKeo.Modal;
using System.Threading.Tasks;
using OfficeOpenXml;
using System.IO;

namespace DuAnBanBanhKeo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SanPhamController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly string _uploadPath;

        public SanPhamController(ApplicationDbContext context)
        {
            _context = context;
            _uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
            if (!Directory.Exists(_uploadPath))
            {
                Directory.CreateDirectory(_uploadPath);
            }
        }

        // GET: api/SanPham
        [HttpGet]
        public async Task<ActionResult<object>> GetSanPhams(
            string? searchTerm,
            string? maDanhMucFilter,
            bool? trangThaiFilter,
            string? maNCCFilter,
            string? sortBy,
            string? sortOrder = "asc",
            int page = 1,
            int pageSize = 10)
        {
            try
            {
                IQueryable<SanPham> query = _context.SanPhams
                    .Include(sp => sp.NhaCungCap)
                    .Include(sp => sp.DanhMuc)
                    .Include(sp => sp.HinhAnhs);
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    searchTerm = searchTerm.ToLower();
                    query = query.Where(sp =>
                        sp.TenSP.ToLower().Contains(searchTerm) ||
                        (sp.NhaCungCap != null && sp.NhaCungCap.TenNCC.ToLower().Contains(searchTerm)) ||
                        (sp.DanhMuc != null && sp.DanhMuc.TenDanhMuc.ToLower().Contains(searchTerm))
                    );
                }

                if (!string.IsNullOrEmpty(maDanhMucFilter))
                {
                    query = query.Where(sp => sp.MaDanhMuc == maDanhMucFilter);
                }

                if (trangThaiFilter.HasValue)
                {
                    query = query.Where(sp => sp.TrangThai == trangThaiFilter.Value);
                }

                if (!string.IsNullOrEmpty(maNCCFilter))
                {
                    query = query.Where(sp => sp.MaNCC == maNCCFilter);
                }

                if (!string.IsNullOrEmpty(sortBy))
                {
                    if (sortBy.ToLower() == "tensp")
                    {
                        query = sortOrder.ToLower() == "desc" ? query.OrderByDescending(sp => sp.TenSP) : query.OrderBy(sp => sp.TenSP);
                    }
                }
                else
                {
                    query = query.OrderBy(sp => sp.MaSP);
                }

                int totalItems = await query.CountAsync();
                int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

                var sanPhams = await query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var sanPhamDtos = sanPhams.Select(sp => new SanPhamForViewDto
                {
                    MaSP = sp.MaSP,
                    TenSP = sp.TenSP,
                    HinhAnhDaiDien = sp.HinhAnhs.FirstOrDefault()?.Url,
                    TenDanhMuc = sp.DanhMuc?.TenDanhMuc,
                    TenNCC = sp.NhaCungCap?.TenNCC,
                    SoLuongTon = sp.SoLuongTon,
                    TrangThai = sp.SoLuongTon > 10 ? "Còn hàng" : (sp.SoLuongTon > 0 ? "Sắp hết hàng" : "Hết hàng")
                }).ToList();



                return Ok(new
                {
                    Items = sanPhamDtos,
                    TotalItems = totalItems,
                    TotalPages = totalPages,
                    PageSize = pageSize,
                    CurrentPage = page
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/SanPham/MASP
        [HttpGet("{maSP}")]
        public async Task<ActionResult<SanPham>> GetSanPham(string maSP)
        {
            var sanPham = await _context.SanPhams
                .Include(sp => sp.HinhAnhs)
                .Include(sp => sp.DanhMuc)
                .Include(sp => sp.NhaCungCap)
                .FirstOrDefaultAsync(sp => sp.MaSP == maSP);

            if (sanPham == null)
            {
                return NotFound();
            }

            return Ok(sanPham);
        }

        // POST: api/SanPham
        [HttpPost]
        public async Task<ActionResult<SanPham>> PostSanPham(
            [FromForm] SanPham sanPham,
            [FromForm] List<IFormFile> hinhAnhFiles)
        {
            Debug.WriteLine("PostSanPham action START");
            Debug.WriteLine($"SanPham model state is valid: {ModelState.IsValid}");

            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Debug.WriteLine($"Validation Error: {error.ErrorMessage}");
                }
                return BadRequest(ModelState);
            }

            string maSPMoi = await GenerateMaSP();
            sanPham.MaSP = maSPMoi;
            Debug.WriteLine($"Generated MaSP: {maSPMoi}");

            if (SanPhamExists(sanPham.MaSP))
            {
                Debug.WriteLine($"Conflict: SanPham with MaSP '{sanPham.MaSP}' already exists.");
                return Conflict("Mã sản phẩm đã tồn tại.");
            }

            _context.SanPhams.Add(sanPham);
            Debug.WriteLine($"SanPham added to context: {sanPham.MaSP}");

            try
            {
                await _context.SaveChangesAsync();

                if (hinhAnhFiles != null && hinhAnhFiles.Any())
                {
                    foreach (var formFile in hinhAnhFiles)
                    {
                        if (formFile.Length > 0)
                        {
                            // Generate a unique file name
                            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(formFile.FileName);
                            var filePath = Path.Combine(_uploadPath, fileName);

                            // Save the file to the server
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await formFile.CopyToAsync(stream);
                            }

                            // Create a new HinhAnh record with the file path
                            var hinhAnh = new HinhAnh
                            {
                                Url = $"/uploads/{fileName}",
                                MaSP = sanPham.MaSP
                            };
                            _context.HinhAnhs.Add(hinhAnh);
                        }
                    }
                    await _context.SaveChangesAsync();
                }
            }
            catch (DbUpdateException ex)
            {
                Debug.WriteLine($"DbUpdateException: {ex.Message}");
                Debug.WriteLine($"Inner Exception: {ex.InnerException?.Message}");
                if (SanPhamExists(sanPham.MaSP))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            Debug.WriteLine("PostSanPham action END");
            return CreatedAtAction("GetSanPham", new { maSP = sanPham.MaSP }, sanPham);
        }

        // PUT: api/SanPham/MASP
        [HttpPut("{maSP}")]
        public async Task<IActionResult> PutSanPham(string maSP, [FromForm] SanPham sanPhamUpdate, [FromForm] List<IFormFile> hinhAnhFiles)
        {
            if (maSP != sanPhamUpdate.MaSP)
            {
                return BadRequest("Mã sản phẩm không khớp.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingSanPham = await _context.SanPhams
                .Include(sp => sp.HinhAnhs)
                .FirstOrDefaultAsync(sp => sp.MaSP == maSP);

            if (existingSanPham == null)
            {
                return NotFound();
            }

            // Update the product details
            existingSanPham.TenSP = sanPhamUpdate.TenSP;
            existingSanPham.GiaNhap = sanPhamUpdate.GiaNhap;
            existingSanPham.GiaBan = sanPhamUpdate.GiaBan;
            existingSanPham.SoLuongTon = sanPhamUpdate.SoLuongTon;
            existingSanPham.DonViTinh = sanPhamUpdate.DonViTinh;
            existingSanPham.TrangThai = sanPhamUpdate.TrangThai;
            existingSanPham.MaDanhMuc = sanPhamUpdate.MaDanhMuc;
            existingSanPham.MaNCC = sanPhamUpdate.MaNCC;
            existingSanPham.NgayNhap = sanPhamUpdate.NgayNhap;
            existingSanPham.NgayHetHan = sanPhamUpdate.NgayHetHan;

            // Delete existing images from the server and database
            if (existingSanPham.HinhAnhs != null && existingSanPham.HinhAnhs.Any())
            {
                foreach (var hinhAnh in existingSanPham.HinhAnhs)
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", hinhAnh.Url.TrimStart('/'));
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }
                _context.HinhAnhs.RemoveRange(existingSanPham.HinhAnhs);
            }
            existingSanPham.HinhAnhs.Clear();

            // Add new images
            if (hinhAnhFiles != null && hinhAnhFiles.Any())
            {
                foreach (var formFile in hinhAnhFiles)
                {
                    if (formFile.Length > 0)
                    {
                        // Generate a unique file name
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(formFile.FileName);
                        var filePath = Path.Combine(_uploadPath, fileName);

                        // Save the file to the server
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await formFile.CopyToAsync(stream);
                        }

                        // Create a new HinhAnh record with the file path
                        var hinhAnh = new HinhAnh
                        {
                            Url = $"/uploads/{fileName}",
                            MaSP = maSP
                        };
                        _context.HinhAnhs.Add(hinhAnh);
                    }
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SanPhamExists(maSP))
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

        // DELETE: api/SanPham/MASP
        [HttpDelete("{maSP}")]
        public async Task<IActionResult> DeleteSanPham(string maSP)
        {
            var sanPham = await _context.SanPhams
                .Include(sp => sp.HinhAnhs)
                .FirstOrDefaultAsync(sp => sp.MaSP == maSP);

            if (sanPham == null)
            {
                return NotFound();
            }

            // Delete associated images from the server and database
            if (sanPham.HinhAnhs != null && sanPham.HinhAnhs.Any())
            {
                foreach (var hinhAnh in sanPham.HinhAnhs)
                {
                    // Construct the full file path
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", hinhAnh.Url.TrimStart('/'));
                    try
                    {
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                            Console.WriteLine($"Deleted file: {filePath}");
                        }
                        else
                        {
                            Console.WriteLine($"File not found: {filePath}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error deleting file {filePath}: {ex.Message}");
                        // Optionally log the error, but continue with deletion
                    }
                }
                // Remove HinhAnh records from the database
                _context.HinhAnhs.RemoveRange(sanPham.HinhAnhs);
            }

            // Remove the product from the database
            _context.SanPhams.Remove(sanPham);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("export-excel")]
        public async Task<IActionResult> ExportSanPhamsToExcel(
            string? searchTerm,
            string? maDanhMucFilter,
            bool? trangThaiFilter,
            string? maNCCFilter,
            string? sortBy,
            string? sortOrder = "asc")
        {
            try
            {
                IQueryable<SanPham> query = _context.SanPhams
                    .Include(sp => sp.NhaCungCap)
                    .Include(sp => sp.DanhMuc)
                    .Include(sp => sp.HinhAnhs);

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    searchTerm = searchTerm.ToLower();
                    query = query.Where(sp =>
                        sp.TenSP.ToLower().Contains(searchTerm) ||
                        (sp.NhaCungCap != null && sp.NhaCungCap.TenNCC.ToLower().Contains(searchTerm)) ||
                        (sp.DanhMuc != null && sp.DanhMuc.TenDanhMuc.ToLower().Contains(searchTerm)));
                }

                if (!string.IsNullOrEmpty(maDanhMucFilter))
                {
                    query = query.Where(sp => sp.MaDanhMuc == maDanhMucFilter);
                }

                if (trangThaiFilter.HasValue)
                {
                    query = query.Where(sp => sp.TrangThai == trangThaiFilter.Value);
                }

                if (!string.IsNullOrEmpty(maNCCFilter))
                {
                    query = query.Where(sp => sp.MaNCC == maNCCFilter);
                }

                if (!string.IsNullOrEmpty(sortBy))
                {
                    if (sortBy.ToLower() == "tensp")
                    {
                        query = sortOrder.ToLower() == "desc" ? query.OrderByDescending(sp => sp.TenSP) : query.OrderBy(sp => sp.TenSP);
                    }
                }
                else
                {
                    query = query.OrderBy(sp => sp.MaSP);
                }

                var sanPhams = await query.ToListAsync();

                // Tạo file Excel bằng EPPlus
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("DanhSachSanPhamChiTiet");

                    // Tiêu đề cột
                    string[] headers = new string[]
                    {
                "Mã Sản Phẩm", "Tên Sản Phẩm", "Giá Nhập", "Giá Bán", "Số Lượng Tồn", "Đơn Vị Tính",
                "Trạng Thái", "Ngày Nhập", "Ngày Hết Hạn", "Danh Mục", "Nhà Cung Cấp", "Hình Ảnh"
                    };
                    for (int i = 0; i < headers.Length; i++)
                    {
                        worksheet.Cells[1, i + 1].Value = headers[i];
                    }

                    // Định dạng tiêu đề
                    using (var range = worksheet.Cells[1, 1, 1, headers.Length])
                    {
                        range.Style.Font.Bold = true;
                        range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                    }

                    // Thêm dữ liệu
                    int row = 2;
                    foreach (var sp in sanPhams)
                    {
                        worksheet.Cells[row, 1].Value = sp.MaSP;
                        worksheet.Cells[row, 2].Value = sp.TenSP;
                        worksheet.Cells[row, 3].Value = sp.GiaNhap;
                        worksheet.Cells[row, 3].Style.Numberformat.Format = "#,##0.00"; // Định dạng tiền tệ
                        worksheet.Cells[row, 4].Value = sp.GiaBan;
                        worksheet.Cells[row, 4].Style.Numberformat.Format = "#,##0.00"; // Định dạng tiền tệ
                        worksheet.Cells[row, 5].Value = sp.SoLuongTon;
                        worksheet.Cells[row, 6].Value = sp.DonViTinh ?? "Không có";

                        // Xác định trạng thái dựa trên số lượng tồn kho
                        string trangThai;
                        System.Drawing.Color color;
                        if (sp.SoLuongTon > 10)
                        {
                            trangThai = "Còn hàng";
                            color = System.Drawing.Color.Green;
                        }
                        else if (sp.SoLuongTon > 0)
                        {
                            trangThai = "Sắp hết hàng";
                            color = System.Drawing.Color.Yellow;
                        }
                        else
                        {
                            trangThai = "Hết hàng";
                            color = System.Drawing.Color.Red;
                        }
                        worksheet.Cells[row, 7].Value = trangThai;
                        worksheet.Cells[row, 7].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        worksheet.Cells[row, 7].Style.Fill.BackgroundColor.SetColor(color);

                        worksheet.Cells[row, 8].Value = sp.NgayNhap?.ToString("dd/MM/yyyy") ?? "Không có";
                        worksheet.Cells[row, 9].Value = sp.NgayHetHan?.ToString("dd/MM/yyyy") ?? "Không có";
                        worksheet.Cells[row, 10].Value = sp.DanhMuc?.TenDanhMuc ?? "Không có";
                        worksheet.Cells[row, 11].Value = sp.NhaCungCap?.TenNCC ?? "Không có";

                        // Danh sách URL hình ảnh
                        if (sp.HinhAnhs != null && sp.HinhAnhs.Any())
                        {
                            var imageUrls = string.Join("; ", sp.HinhAnhs.Select(h => $"{Request.Scheme}://{Request.Host}{h.Url}"));
                            worksheet.Cells[row, 12].Value = imageUrls;
                        }
                        else
                        {
                            worksheet.Cells[row, 12].Value = "Không có hình ảnh";
                        }

                        row++;
                    }

                    // Tự động điều chỉnh kích thước cột
                    worksheet.Cells.AutoFitColumns();

                    var stream = new MemoryStream();
                    package.SaveAs(stream);
                    stream.Position = 0;

                    string fileName = $"DanhSachSanPhamChiTiet_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                    return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi khi xuất Excel: {ex.Message}");
            }
        }

        private bool SanPhamExists(string maSP)
        {
            return _context.SanPhams.Any(e => e.MaSP == maSP);
        }

        private async Task<string> GenerateMaSP()
        {
            string prefix = "SP";
            const string chars = "0123456789";
            var random = new Random();

            while (true)
            {
                string randomCode = new string(Enumerable.Repeat(chars, 3)
                                                         .Select(s => s[random.Next(s.Length)]).ToArray());
                string maSPMoi = prefix + randomCode;

                if (!SanPhamExists(maSPMoi))
                {
                    return maSPMoi;
                }
            }
        }
    }
}