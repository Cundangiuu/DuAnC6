using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DuAnBanBanhKeo.Data;
using DuAnBanBanhKeo.Data.Entities;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.IO; // Import this

namespace DuAnBanBanhKeo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NhanVienController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<NhanVienController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment; // Add this

        public NhanVienController(ApplicationDbContext context, ILogger<NhanVienController> logger, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
        }

        // 1. Lấy danh sách nhân viên (GET all)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NhanVien>>> GetAllNhanViens()
        {
            try
            {
                return await _context.NhanViens.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách nhân viên.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // 2. Lấy thông tin nhân viên theo ID (GET by ID)
        [HttpGet("{id}")]
        public async Task<ActionResult<NhanVienViewModel>> GetNhanVien(string id)
        {
            try
            {
                var nhanVien = await _context.NhanViens
                    .Include(nv => nv.TaiKhoan) // Load thông tin tài khoản
                    .FirstOrDefaultAsync(nv => nv.MaNV == id);

                if (nhanVien == null)
                {
                    return NotFound(); // Mã 404
                }

                // Tạo ViewModel từ entity
                var nhanVienViewModel = new NhanVienViewModel
                {
                    MaNV = nhanVien.MaNV,
                    HoTen = nhanVien.HoTen,
                    VaiTro = nhanVien.VaiTro,
                    SoDienThoai = nhanVien.SoDienThoai,
                    Email = nhanVien.Email,
                    TrangThai = nhanVien.TrangThai,
                    TenDangNhap = nhanVien.TaiKhoan?.TenDangNhap, // Sử dụng null-conditional operator
                    MatKhau = nhanVien.TaiKhoan?.MatKhau,
                    TrangThaiTK = nhanVien.TaiKhoan?.TrangThai ?? false, // Cẩn thận với kiểu boolean có thể null
                    HinhAnh = nhanVien.HinhAnh // Chỉ trả về tên tệp
                };

                return nhanVienViewModel;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy thông tin nhân viên với ID: {id}", id); // Log lỗi
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        //3. Thêm nhân viên mới (POST)
        [HttpPost]
        public async Task<ActionResult<NhanVien>> CreateNhanVien([FromForm] NhanVienViewModel nhanVienViewModel)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Dữ liệu không hợp lệ khi tạo nhân viên. Errors: {errors}", ModelState);
                return BadRequest(ModelState);
            }

            try
            {
                string uniqueFileName = null;
                if (nhanVienViewModel.HinhAnhFile != null)
                {
                    // **Lưu vào thư mục tạm thời**
                    string tempUploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "temp_uploads", "nhanVien");
                    _logger.LogInformation("Giá trị WebRootPath: {0}", _webHostEnvironment.WebRootPath); // Gỡ lỗi
                    _logger.LogInformation("Giá trị ContentRootPath: {0}", _webHostEnvironment.ContentRootPath); // Gỡ lỗi

                    if (!Directory.Exists(tempUploadsFolder))
                    {
                        Directory.CreateDirectory(tempUploadsFolder);
                        _logger.LogInformation("Tạo thư mục thành công: {0}", tempUploadsFolder); // Gỡ lỗi
                    }

                    uniqueFileName = Guid.NewGuid().ToString() + "_" + nhanVienViewModel.HinhAnhFile.FileName;
                    string filePath = Path.Combine(tempUploadsFolder, uniqueFileName);
                    _logger.LogInformation("Đường dẫn file tạm: {filePath}", filePath);

                    try
                    {
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await nhanVienViewModel.HinhAnhFile.CopyToAsync(fileStream);
                        }
                        _logger.LogInformation("Lưu file thành công vào thư mục tạm: {0}", filePath); // Gỡ lỗi
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Lỗi khi save file vào thư mục tạm tại: {path}", filePath);
                        return StatusCode(StatusCodes.Status500InternalServerError, "Lỗi khi lưu tệp hình ảnh vào thư mục tạm.");
                    }

                    //**Sao chép sang thư mục FE**
                    //Dựa vào cấu trúc thư mục, đường dẫn mới sẽ như sau:
                    string fePath = Path.Combine(_webHostEnvironment.ContentRootPath, "..", "FRONT-END", "Admin", "src", "assets", "images", "nhanVien", uniqueFileName);

                    // Đảm bảo thư mục đích tồn tại
                    string feDirectory = Path.GetDirectoryName(fePath);
                    _logger.LogInformation("Đường dẫn thư mục FE: {feDirectory}", feDirectory);
                    if (!Directory.Exists(feDirectory))
                    {
                        Directory.CreateDirectory(feDirectory);
                        _logger.LogInformation("Đã tạo thư mục FE đích: {path}", feDirectory);
                    }

                    try
                    {
                        // Kiểm tra xem tệp nguồn có tồn tại hay không
                        if (!System.IO.File.Exists(filePath))
                        {
                            _logger.LogError("Tệp nguồn không tồn tại: {filePath}", filePath);
                            return StatusCode(StatusCodes.Status500InternalServerError, "Tệp nguồn không tồn tại.");
                        }
                        _logger.LogInformation("Sao chép file từ {filePath} đến {fePath}", filePath, fePath);
                        System.IO.File.Copy(filePath, fePath, true);
                        _logger.LogInformation("Sao chép file thành công!");
                    }
                    catch (IOException copyEx)
                    {
                        _logger.LogError(copyEx, "Lỗi IOException khi sao chép file từ {0} sang {1}: {Message}", filePath, fePath, copyEx.Message);

                        // Lấy thông tin bổ sung về lỗi (nếu có)
                        if (copyEx.InnerException != null)
                        {
                            _logger.LogError(copyEx.InnerException, "Inner Exception: {Message}", copyEx.InnerException.Message);
                        }

                        return StatusCode(StatusCodes.Status500InternalServerError, "Lỗi khi sao chép file sang thư mục FRONT-ENT");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Lỗi chung khi sao chép file từ {0} sang {1}", filePath, fePath);
                        return StatusCode(StatusCodes.Status500InternalServerError, "Lỗi khi sao chép file sang thư mục FRONT-ENT");
                    }
                }

                // Create nhân viên mới
                var nhanVien = new NhanVien
                {
                    MaNV = nhanVienViewModel.MaNV,
                    HoTen = nhanVienViewModel.HoTen,
                    VaiTro = nhanVienViewModel.VaiTro,
                    SoDienThoai = nhanVienViewModel.SoDienThoai,
                    Email = nhanVienViewModel.Email,
                    TrangThai = nhanVienViewModel.TrangThai,
                    HinhAnh = uniqueFileName  // Chỉ lưu tên tệp
                };

                // Create tài khoản mới
                var taiKhoan = new TaiKhoan
                {
                    MaTK = GenerateMaTK(),
                    TenDangNhap = nhanVienViewModel.TenDangNhap,
                    MatKhau = nhanVienViewModel.MatKhau,
                    MaNV = nhanVien.MaNV,
                    TrangThai = nhanVienViewModel.TrangThaiTK
                };

                nhanVien.TaiKhoan = taiKhoan;

                _context.NhanViens.Add(nhanVien);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetNhanVien), new { id = nhanVien.MaNV }, nhanVien);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tạo nhân viên.");
                Debug.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // 4. Cập nhật thông tin nhân viên (PUT)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNhanVien(string id, [FromForm] NhanVienViewModel nhanVienViewModel)
        {
            if (id != nhanVienViewModel.MaNV)
            {
                return BadRequest("ID in request body does not match ID in route.");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Dữ liệu không hợp lệ khi cập nhật nhân viên. Errors: {errors}", ModelState);
                return BadRequest(ModelState);
            }

            try
            {
                var nhanVien = await _context.NhanViens
                    .Include(nv => nv.TaiKhoan)
                    .FirstOrDefaultAsync(nv => nv.MaNV == id);

                if (nhanVien == null)
                {
                    return NotFound();
                }

                // Cập nhật thông tin nhân viên
                nhanVien.HoTen = nhanVienViewModel.HoTen;
                nhanVien.VaiTro = nhanVienViewModel.VaiTro;
                nhanVien.SoDienThoai = nhanVienViewModel.SoDienThoai;
                nhanVien.Email = nhanVienViewModel.Email;
                nhanVien.TrangThai = nhanVienViewModel.TrangThai;

                // Handle Image Update
                if (nhanVienViewModel.HinhAnhFile != null)
                {
                    // **Xóa ảnh cũ từ thư mục tạm**
                    if (nhanVien.HinhAnh != null)
                    {
                        string oldPath = Path.Combine(_webHostEnvironment.WebRootPath, "temp_uploads", "nhanVien", nhanVien.HinhAnh);
                        if (System.IO.File.Exists(oldPath))
                        {
                            try
                            {
                                System.IO.File.Delete(oldPath);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, "Không thể xóa tệp cũ: {path}", oldPath);
                                return StatusCode(StatusCodes.Status500InternalServerError, "Không thể xóa tệp hình ảnh cũ.");
                            }
                        }
                    }

                    // **Lưu ảnh mới vào thư mục tạm**
                    string tempUploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "temp_uploads", "nhanVien");
                    if (!Directory.Exists(tempUploadsFolder))
                    {
                        try
                        {
                            Directory.CreateDirectory(tempUploadsFolder);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Không thể tạo thư mục: {path}", tempUploadsFolder);
                            return StatusCode(StatusCodes.Status500InternalServerError, "Không thể tạo thư mục lưu trữ tạm thời.");
                        }
                    }

                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + nhanVienViewModel.HinhAnhFile.FileName;
                    string filePath = Path.Combine(tempUploadsFolder, uniqueFileName);
                    _logger.LogInformation("Đường dẫn file tạm: {filePath}", filePath);

                    try
                    {
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await nhanVienViewModel.HinhAnhFile.CopyToAsync(fileStream);
                        }
                        _logger.LogInformation("Lưu file thành công vào thư mục tạm: {0}", filePath); // Gỡ lỗi
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Lỗi khi save file vào thư mục tạm tại: {path}", filePath);
                        return StatusCode(StatusCodes.Status500InternalServerError, "Lỗi khi lưu tệp hình ảnh.");
                    }
                    //**Sao chép sang thư mục FE**
                    //Dựa vào cấu trúc thư mục, đường dẫn mới sẽ như sau:
                    string fePath = Path.Combine(_webHostEnvironment.ContentRootPath, "..", "FRONT-END", "Admin", "src", "assets", "images", "nhanVien", uniqueFileName);
                    // Đảm bảo thư mục đích tồn tại
                    string feDirectory = Path.GetDirectoryName(fePath);
                    _logger.LogInformation("Đường dẫn thư mục FE: {feDirectory}", feDirectory);
                    if (!Directory.Exists(feDirectory))
                    {
                        Directory.CreateDirectory(feDirectory);
                        _logger.LogInformation("Đã tạo thư mục FE đích: {path}", feDirectory);
                    }

                    try
                    {
                        // Kiểm tra xem tệp nguồn có tồn tại hay không
                        if (!System.IO.File.Exists(filePath))
                        {
                            _logger.LogError("Tệp nguồn không tồn tại: {filePath}", filePath);
                            return StatusCode(StatusCodes.Status500InternalServerError, "Tệp nguồn không tồn tại.");
                        }
                        _logger.LogInformation("Sao chép file từ {filePath} đến {fePath}", filePath, fePath);
                        System.IO.File.Copy(filePath, fePath, true);
                        _logger.LogInformation("Sao chép file thành công!");
                    }
                    catch (IOException copyEx)
                    {
                        _logger.LogError(copyEx, "Lỗi IOException khi sao chép file từ {0} sang {1}: {Message}", filePath, fePath, copyEx.Message);

                        // Lấy thông tin bổ sung về lỗi (nếu có)
                        if (copyEx.InnerException != null)
                        {
                            _logger.LogError(copyEx.InnerException, "Inner Exception: {Message}", copyEx.InnerException.Message);
                        }

                        return StatusCode(StatusCodes.Status500InternalServerError, "Lỗi khi sao chép file sang thư mục FRONT-ENT");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Lỗi chung khi sao chép file từ {0} sang {1}", filePath, fePath);
                        return StatusCode(StatusCodes.Status500InternalServerError, "Lỗi khi sao chép file sang thư mục FRONT-ENT");
                    }
                    nhanVien.HinhAnh = uniqueFileName;
                }

                // Cập nhật hoặc tạo thông tin tài khoản
                if (nhanVien.TaiKhoan != null)
                {
                    nhanVien.TaiKhoan.TenDangNhap = nhanVienViewModel.TenDangNhap;
                    nhanVien.TaiKhoan.MatKhau = nhanVienViewModel.MatKhau;
                    nhanVien.TaiKhoan.TrangThai = nhanVienViewModel.TrangThaiTK;
                }
                else
                {
                    var taiKhoan = new TaiKhoan
                    {
                        MaTK = GenerateMaTK(),
                        TenDangNhap = nhanVienViewModel.TenDangNhap,
                        MatKhau = nhanVienViewModel.MatKhau,
                        MaNV = nhanVien.MaNV,
                        TrangThai = nhanVienViewModel.TrangThaiTK
                    };
                    nhanVien.TaiKhoan = taiKhoan;
                }

                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!_context.NhanViens.Any(e => e.MaNV == id))
                {
                    return NotFound();
                }
                else
                {
                    _logger.LogError(ex, "Lỗi concurrency khi cập nhật nhân viên.");
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi cập nhật nhân viên.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpPatch("{id}")] // Sử dụng HttpPatch để cập nhật một phần
        public async Task<IActionResult> ToggleNhanVienStatus(string id)
        {
            try
            {
                var nhanVien = await _context.NhanViens.Include(nv => nv.TaiKhoan).FirstOrDefaultAsync(nv => nv.MaNV == id);
                if (nhanVien == null)
                {
                    _logger.LogWarning("Không tìm thấy nhân viên với ID: {id}", id);
                    return NotFound();
                }

                // Đảo ngược trạng thái của nhân viên
                nhanVien.TrangThai = !nhanVien.TrangThai;

                // Đảo ngược trạng thái của tài khoản (nếu có)
                if (nhanVien.TaiKhoan != null)
                {
                    nhanVien.TaiKhoan.TrangThai = !nhanVien.TaiKhoan.TrangThai;
                }

                await _context.SaveChangesAsync();

                return NoContent(); // Trả về mã 204 No Content khi thành công
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi cập nhật trạng thái nhân viên: " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        private string GenerateMaTK()
        {
            try
            {
                // Lấy MaTK lớn nhất hiện có
                string lastMaTK = _context.TaiKhoans
                    .OrderByDescending(tk => tk.MaTK)
                    .Select(tk => tk.MaTK)
                    .FirstOrDefault();

                int nextNumber = 1;
                if (!string.IsNullOrEmpty(lastMaTK) && lastMaTK.StartsWith("TK"))
                {
                    // Tách số từ MaTK cuối cùng
                    if (int.TryParse(lastMaTK.Substring(2), out int lastNumber))
                    {
                        nextNumber = lastNumber + 1;
                    }
                }

                return "TK" + nextNumber.ToString("D3");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tạo mã tài khoản tự động.");
                return Guid.NewGuid().ToString(); // Trả về Guid nếu có lỗi
            }
        }
    }

    //ViewModel
    public class NhanVienViewModel
    {
        [Required(ErrorMessage = "Mã nhân viên là bắt buộc.")]
        [StringLength(10, ErrorMessage = "Mã nhân viên không được vượt quá 10 ký tự.")]
        public string MaNV { get; set; }
        public string? HinhAnh { get; set; } // Chỉ lưu tên tệp

        [Required(ErrorMessage = "Họ tên là bắt buộc.")]
        [StringLength(100, ErrorMessage = "Họ tên không được vượt quá 100 ký tự.")]
        public string HoTen { get; set; }

        public string VaiTro { get; set; }

        [Required(ErrorMessage = "Số điện thoại là bắt buộc.")]
        [RegularExpression(@"^0[0-9]{9}$", ErrorMessage = "Số điện thoại không hợp lệ.")]
        public string SoDienThoai { get; set; }

        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        public string Email { get; set; }
        public bool TrangThai { get; set; }

        //Thông tin tài khoản
        [Required(ErrorMessage = "Tên đăng nhập là bắt buộc.")]
        public string TenDangNhap { get; set; }

        [Required(ErrorMessage = "Mật khẩu là bắt buộc.")]
        public string MatKhau { get; set; }

        public bool TrangThaiTK { get; set; }

        // Image properties

        [DataType(DataType.Upload)]
        public IFormFile? HinhAnhFile { get; set; }
    }
}