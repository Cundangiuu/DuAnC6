using DuAnBanBanhKeo.Data;
using DuAnBanBanhKeo.Data.Dtos;
using DuAnBanBanhKeo.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace DuAnBanBanhKeo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HinhAnhController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly string _uploadPath;

        public HinhAnhController(ApplicationDbContext context)
        {
            _context = context;
            // Define the path to store uploaded images
            _uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
            if (!Directory.Exists(_uploadPath))
            {
                Directory.CreateDirectory(_uploadPath);
            }
        }

        // GET: api/HinhAnh/SanPham/MASP
        [HttpGet("SanPham/{maSP}")]
        public async Task<ActionResult<IEnumerable<HinhAnh>>> GetHinhAnhsBySanPhamId(string maSP)
        {
            var hinhAnhs = await _context.HinhAnhs
                .Where(ha => ha.MaSP == maSP)
                .ToListAsync();

            return Ok(hinhAnhs);
        }

        // GET: api/HinhAnh/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HinhAnh>> GetHinhAnh(int id)
        {
            var hinhAnh = await _context.HinhAnhs.FindAsync(id);

            if (hinhAnh == null)
            {
                return NotFound();
            }

            return Ok(hinhAnh);
        }

        // POST: api/HinhAnh
        [HttpPost]
        public async Task<ActionResult<HinhAnh>> PostHinhAnh([FromForm] HinhAnhUploadDto model)
        {
            if (model.File == null || model.File.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            // Generate a unique file name
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.File.FileName);
            var filePath = Path.Combine(_uploadPath, fileName);

            // Save the file to the server
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.File.CopyToAsync(stream);
            }

            // Lưu URL đầy đủ với domain/port của backend
            var baseUrl = $"{Request.Scheme}://{Request.Host}"; // Lấy scheme (http/https) và host (localhost:7203)
            var hinhAnh = new HinhAnh
            {
                Url = $"{baseUrl}/uploads/{fileName}", // URL đầy đủ, ví dụ: https://localhost:7203/uploads/xxx.png
                MaSP = model.MaSP
            };

            _context.HinhAnhs.Add(hinhAnh);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetHinhAnh", new { id = hinhAnh.Id }, hinhAnh);
        }

        // DELETE: api/HinhAnh/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHinhAnh(int id)
        {
            var hinhAnh = await _context.HinhAnhs.FindAsync(id);
            if (hinhAnh == null)
            {
                return NotFound();
            }

            // Delete the file from the server
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

            // Remove the HinhAnh record from the database
            _context.HinhAnhs.Remove(hinhAnh);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool HinhAnhExists(int id)
        {
            return _context.HinhAnhs.Any(e => e.Id == id);
        }
    }
}