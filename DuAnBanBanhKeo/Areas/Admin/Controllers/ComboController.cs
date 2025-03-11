using DuAnBanBanhKeo.Data.Entities;
using DuAnBanBanhKeo.Responsive;
using DuAnBanBanhKeo.Services;
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
        public async Task<IActionResult> CreateCombo([FromBody] Combo combo, [FromBody] List<ChiTietCombo> chiTietCombos)
        {
            if (combo == null || chiTietCombos == null)
            {
                return BadRequest("Combo hoặc ChiTietCombo không được để trống");
            }

            await _comboServices.Add(combo, chiTietCombos);
            return CreatedAtAction(nameof(GetComboById), new { id = combo.MaCombo }, combo);
        }


        // PUT: api/Combo/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCombo(string id, [FromBody] Combo combo, [FromBody] List<ChiTietCombo> chiTietCombos)
        {
            if (combo == null || combo.MaCombo != id)
            {
                return BadRequest("Dữ liệu không hợp lệ");
            }

            await _comboServices.Update(combo, chiTietCombos);
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
