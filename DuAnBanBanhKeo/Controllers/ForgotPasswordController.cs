using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Linq;
using System.Threading.Tasks;
using DuAnBanBanhKeo.Modal;
using DuAnBanBanhKeo.Services;
using Microsoft.Extensions.Logging;
using BCrypt.Net;
using DuAnBanBanhKeo.Data;
using Microsoft.EntityFrameworkCore;
using DuAnBanBanhKeo.Data.Entities; // Import namespace cho Entities

[Route("api/[controller]")]
[ApiController]
public class ForgotPasswordController : ControllerBase
{
    private readonly IMemoryCache _cache;
    private readonly IEmailService _emailService;
    private readonly ILogger<ForgotPasswordController> _logger;
    private readonly ApplicationDbContext _context;

    public ForgotPasswordController(
        IMemoryCache cache,
        IEmailService emailService,
        ILogger<ForgotPasswordController> logger,
        ApplicationDbContext context)
    {
        _cache = cache;
        _emailService = emailService;
        _logger = logger;
        _context = context;
    }

    [HttpPost("SendOtp")]
    public async Task<IActionResult> SendOtp([FromBody] ForgotPasswordRequest model)
    {
        try
        {
            if (string.IsNullOrEmpty(model.Email) || !model.Email.Contains("@"))
            {
                return BadRequest(new { success = false, message = "Email không hợp lệ." });
            }

            // *Kiểm tra Email tồn tại trong bảng NhanVien*
            var nhanVien = await _context.NhanViens.FirstOrDefaultAsync(n => n.Email == model.Email);
            if (nhanVien == null)
            {
                return BadRequest(new { success = false, message = "Email không tồn tại trong hệ thống." });
            }

            var otpCode = new Random().Next(100000, 999999).ToString();
            var otpModel = new OtpModel
            {
                Email = model.Email,
                OtpCode = otpCode,
                ExpiryTime = DateTime.UtcNow.AddMinutes(5)
            };

            _cache.Set(model.Email, otpModel, TimeSpan.FromMinutes(5));
            await _emailService.SendEmailAsync(model.Email, "Mã OTP đặt lại mật khẩu", $"Mã OTP của bạn là: {otpCode}. Mã có hiệu lực trong 5 phút.");
            _logger.LogInformation($"OTP đã được gửi tới email: {model.Email}");

            return Ok(new { success = true, message = "Mã OTP đã được gửi đến email." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi gửi OTP.");
            return StatusCode(500, new { success = false, message = "Đã xảy ra lỗi khi gửi OTP." });
        }
    }

    [HttpPost("VerifyOtp")]
    public IActionResult VerifyOtp([FromBody] VerifyOtpRequest model)
    {
        if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Otp))
        {
            return BadRequest(new { success = false, message = "Email hoặc mã OTP không hợp lệ." });
        }

        if (!_cache.TryGetValue(model.Email, out OtpModel otpModel))
        {
            return BadRequest(new { success = false, message = "Mã OTP không tồn tại hoặc đã hết hạn." });
        }

        if (otpModel.OtpCode != model.Otp)
        {
            return BadRequest(new { success = false, message = "Mã OTP không chính xác." });
        }

        return Ok(new { success = true, message = "Mã OTP hợp lệ." });
    }

    [HttpPost("ResetPassword")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest model)
    {

        if (string.IsNullOrEmpty(model.Email))
        {
            return BadRequest(new { success = false, message = "Email không được để trống." });
        }

        if (string.IsNullOrEmpty(model.Otp))
        {
            return BadRequest(new { success = false, message = "Mã OTP không hợp lệ." });
        }

        if (string.IsNullOrEmpty(model.NewPassword))
        {
            return BadRequest(new { success = false, message = "Mật khẩu mới không hợp lệ." });
        }

        _logger.LogInformation($"Kiểm tra email: {model.Email}");

        if (!_cache.TryGetValue(model.Email, out OtpModel otpModel))
        {
            return BadRequest(new { success = false, message = "OTP đã hết hạn hoặc không tồn tại." });
        }

        if (otpModel.OtpCode != model.Otp)
        {
            return BadRequest(new { success = false, message = "Mã OTP không chính xác." });
        }

        _cache.Remove(model.Email);

        // *Lấy NhanVien bằng Email*
        var nhanVien = await _context.NhanViens.FirstOrDefaultAsync(n => n.Email == model.Email);
        if (nhanVien == null)
        {
            return BadRequest(new { success = false, message = "Email không tồn tại trong hệ thống." });
        }

        // *Lấy TaiKhoan từ bảng TaiKhoan thông qua MaNV*
        var user = await _context.TaiKhoans.FirstOrDefaultAsync(u => u.MaNV == nhanVien.MaNV);
        if (user == null)
        {
            return BadRequest(new { success = false, message = "Không tìm thấy tài khoản liên kết với email này." });
        }

        // Mã hóa mật khẩu mới trước khi lưu
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);

        user.MatKhau = model.NewPassword; // Lưu mật khẩu đã mã hóa 
        await _context.SaveChangesAsync();

        _logger.LogInformation($"Mật khẩu cho email {model.Email} đã được thay đổi thành công.");

        return Ok(new { success = true, message = "Mật khẩu đã được cập nhật thành công." });
    }
}