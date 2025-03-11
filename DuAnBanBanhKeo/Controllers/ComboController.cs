using DuAnBanBanhKeo.Data.Entities;
using DuAnBanBanhKeo.Helper;
using DuAnBanBanhKeo.Responsive;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DuAnBanBanhKeo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComboController : ControllerBase
    {
        private readonly IComboServices _comboServices;

        public ComboController(IComboServices comboServices)
        {
            _comboServices = comboServices;
        }

        // GET: api/Combo
        [HttpGet]
        public async Task<IActionResult> GetCombos()
        {
            var combos = await _comboServices.GetAll();
            return Ok(combos); 
        }

        // GET: api/Combo/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetComboById(string id)
        {
            var combo = await _comboServices.GetById(id);
            if (combo == null)
            {
                return NotFound(); 
            }
            return Ok(combo); 
        }

        // POST: api/Combo
        [HttpPost]
        public async Task<IActionResult> CreateCombo([FromBody] ComboCreateRequest request)
        {
            if (request.Combo == null || request.ChiTietCombos == null)
            {
                return BadRequest("Combo hoặc ChiTietCombo không được để trống");
            }

            await _comboServices.Add(request.Combo, request.ChiTietCombos);
            return CreatedAtAction(nameof(GetComboById), new { id = request.Combo.MaCombo }, request.Combo);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCombo(string id, [FromBody] ComboCreateRequest request)
        {
            if (request.Combo == null || request.Combo.MaCombo != id)
            {
                return BadRequest("Dữ liệu không hợp lệ");
            }

            await _comboServices.Update(request.Combo, request.ChiTietCombos);
            return NoContent();
        }




        // DELETE: api/Combo/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCombo(string id)
        {
            var combo = await _comboServices.GetById(id);
            if (combo == null)
            {
                return NotFound();
            }

            await _comboServices.Delete(id);
            return NoContent(); 
        }
    }
}
