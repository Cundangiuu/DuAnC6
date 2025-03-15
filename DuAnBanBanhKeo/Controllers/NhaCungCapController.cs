using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DuAnBanBanhKeo.Data;
using DuAnBanBanhKeo.Data.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DuAnBanBanhKeo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NhaCungCapController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public NhaCungCapController(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // GET: api/NhaCungCap
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NhaCungCap>>> GetNhaCungCaps(
            [FromQuery] string? searchTerm,
            [FromQuery] string? sortBy,
            [FromQuery] string? sortOrder,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                IQueryable<NhaCungCap> query = _context.NhaCungCaps;

                // Tìm kiếm
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    query = query.Where(ncc =>
                        ncc.MaNCC.Contains(searchTerm) ||
                        ncc.TenNCC.Contains(searchTerm) ||
                        ncc.DiaChi!.Contains(searchTerm) || // Sử dụng null-conditional operator
                        ncc.SoDienThoai!.Contains(searchTerm) ||
                        ncc.Email!.Contains(searchTerm));
                }

                // Sắp xếp
                if (!string.IsNullOrEmpty(sortBy))
                {
                    switch (sortBy.ToLower())
                    {
                        case "mancc":
                            query = sortOrder?.ToLower() == "desc" ? query.OrderByDescending(ncc => ncc.MaNCC) : query.OrderBy(ncc => ncc.MaNCC);
                            break;
                        case "tenncc":
                            query = sortOrder?.ToLower() == "desc" ? query.OrderByDescending(ncc => ncc.TenNCC) : query.OrderBy(ncc => ncc.TenNCC);
                            break;
                        case "diachi":
                            query = sortOrder?.ToLower() == "desc" ? query.OrderByDescending(ncc => ncc.DiaChi) : query.OrderBy(ncc => ncc.DiaChi);
                            break;
                        //Thêm các trường sắp xếp khác nếu cần
                        default:
                            break;
                    }
                }

                // Phân trang
                var totalCount = await query.CountAsync();
                query = query.Skip((page - 1) * pageSize).Take(pageSize);

                var nhaCungCaps = await query.ToListAsync();

                // Tạo metadata phân trang để trả về cho client
                var paginationMetadata = new
                {
                    totalCount,
                    pageSize,
                    currentPage = page,
                    totalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
                };

                // Thêm thông tin phân trang vào header của response
                Response.Headers.Add("X-Pagination", System.Text.Json.JsonSerializer.Serialize(paginationMetadata));

                return Ok(nhaCungCaps);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, "Lỗi khi truy xuất dữ liệu nhà cung cấp.");
            }
        }

        // GET: api/NhaCungCap/NCC001
        [HttpGet("{id}")]
        public async Task<ActionResult<NhaCungCap>> GetNhaCungCap(string id)
        {
            try
            {
                var nhaCungCap = await _context.NhaCungCaps.FindAsync(id);

                if (nhaCungCap == null)
                {
                    return NotFound();
                }

                return nhaCungCap;
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, "Lỗi khi truy xuất dữ liệu nhà cung cấp.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutNhaCungCap(string id, [FromBody] NhaCungCap nhaCungCap)
        {
            if (id != nhaCungCap.MaNCC)
            {
                return BadRequest("ID trong URL không khớp với ID trong body.");
            }

            _context.Entry(nhaCungCap).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NhaCungCapExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Lỗi khi cập nhật dữ liệu. Có thể do xung đột.");
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, "Lỗi khi cập nhật dữ liệu nhà cung cấp.");
            }

            return NoContent();
        }

        // POST: api/NhaCungCap
        [HttpPost]
        public async Task<ActionResult<NhaCungCap>> PostNhaCungCap([FromBody] NhaCungCap nhaCungCap)
        {
            _context.NhaCungCaps.Add(nhaCungCap);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (NhaCungCapExists(nhaCungCap.MaNCC))
                {
                    return Conflict("Nhà cung cấp với ID này đã tồn tại.");
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Lỗi khi tạo nhà cung cấp.");
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, "Lỗi khi tạo nhà cung cấp.");
            }

            return CreatedAtAction("GetNhaCungCap", new { id = nhaCungCap.MaNCC }, nhaCungCap);
        }

        // DELETE: api/NhaCungCap/NCC001
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNhaCungCap(string id)
        {
            try
            {
                var nhaCungCap = await _context.NhaCungCaps.FindAsync(id);
                if (nhaCungCap == null)
                {
                    return NotFound();
                }

                _context.NhaCungCaps.Remove(nhaCungCap);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, "Lỗi khi xóa nhà cung cấp.");
            }
        }

        private bool NhaCungCapExists(string id)
        {
            return _context.NhaCungCaps.Any(e => e.MaNCC == id);
        }
    }
}