using DuAnBanBanhKeo.Data.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DuAnBanBanhKeo.Responsive
{
    public interface IComboServices
    {
        Task<IEnumerable<Combo>> GetAll();
        Task<Combo> GetById(string id);
        Task Add(Combo combo, List<ChiTietCombo> chiTietCombos);
        Task Update(Combo combo, List<ChiTietCombo> chiTietCombos);
        Task Delete(string id);
    }
}
