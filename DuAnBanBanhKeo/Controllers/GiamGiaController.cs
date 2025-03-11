using DuAnBanBanhKeo.Responsive;
using DuAnBanBanhKeo.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DuAnBanBanhKeo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GiamGiaController : ControllerBase
    {
        private readonly IGiamGiaServices _giamGiaServices;

        public GiamGiaController(IGiamGiaServices giamGiaServices)
        {
            _giamGiaServices = giamGiaServices;
        }

        // Lấy tất cả giảm giá
        [HttpGet]
        public async Task<IActionResult> GetAllGiamGia()
        {
            var giamGias = await _giamGiaServices.GetAllGiamGia();
            return Ok(giamGias);
        }

        // Lấy giảm giá theo mã
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGiamGiaById(string id)
        {
            var giamGia = await _giamGiaServices.GetGiamGiaById(id);
            if (giamGia == null)
            {
                return NotFound();
            }
            return Ok(giamGia);
        }

        // Thêm mới giảm giá
        [HttpPost]
        public async Task<IActionResult> AddGiamGia([FromBody] GiamGia giamGia)
        {
            if (giamGia == null)
            {
                return BadRequest("Dữ liệu không hợp lệ");
            }
            await _giamGiaServices.AddGiamGia(giamGia);
            return CreatedAtAction(nameof(GetGiamGiaById), new { id = giamGia.MaGiamGia }, giamGia);
        }

        // Cập nhật giảm giá
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGiamGia(string id, [FromBody] GiamGia giamGia)
        {
            if (giamGia.MaGiamGia != id)
            {
                return BadRequest("Dữ liệu không hợp lệ");
            }
            await _giamGiaServices.UpdateGiamGia(giamGia);
            return NoContent();
        }

        // Xóa giảm giá
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGiamGia(string id)
        {
            await _giamGiaServices.DeleteGiamGia(id);
            return NoContent();
        }

        // Lấy mã nhập giảm giá theo mã giảm giá
        [HttpGet("ma-nhap/{maGiamGia}")]
        public async Task<IActionResult> GetMaNhapGiamGia(string maGiamGia)
        {
            var maNhapGiamGias = await _giamGiaServices.GetMaNhapGiamGiaByGiamGiaId(maGiamGia);
            return Ok(maNhapGiamGias);
        }

        // Đánh dấu mã nhập giảm giá là đã sử dụng
        [HttpPut("ma-nhap/{id}/use")]
        public async Task<IActionResult> MarkMaNhapGiamGiaAsUsed(int id)
        {
            await _giamGiaServices.MarkMaNhapGiamGiaAsUsed(id);
            return NoContent();
        }
    }
}
