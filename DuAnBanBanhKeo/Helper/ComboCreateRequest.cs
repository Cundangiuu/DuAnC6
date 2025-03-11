using DuAnBanBanhKeo.Data.Entities;

namespace DuAnBanBanhKeo.Helper
{
    public class ComboCreateRequest
    {
        public Combo Combo { get; set; }
        public List<ChiTietCombo> ChiTietCombos { get; set; }
    }
}
