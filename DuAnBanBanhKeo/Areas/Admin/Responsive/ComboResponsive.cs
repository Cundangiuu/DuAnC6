using DuAnBanBanhKeo.Data;
using DuAnBanBanhKeo.Data.Entities;
using DuAnBanBanhKeo.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DuAnBanBanhKeo.Responsive
{
    public class ComboResponsive : IComboServices
    {
        private readonly ApplicationDbContext _context;

        public ComboResponsive(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Combo>> GetAll()
        {
            return await _context.Combos.ToListAsync();
        }

        public async Task<Combo> GetById(string id)
        {
            return await _context.Combos.FirstOrDefaultAsync(cb => cb.MaCombo == id);
        }

        public async Task Add(Combo combo, List<ChiTietCombo> chiTietCombos)
        {
            if (combo == null)
                throw new ArgumentNullException(nameof(combo));

            await _context.Combos.AddAsync(combo);
            await _context.SaveChangesAsync();

            foreach (var chiTiet in chiTietCombos)
            {
                var sanPham = await _context.SanPhams.FirstOrDefaultAsync(sp => sp.MaSanPham == chiTiet.MaSanPham);
                if (sanPham == null)
                    throw new Exception($"Sản phẩm với ID {chiTiet.MaSanPham} không tồn tại.");

                chiTiet.MaCombo = combo.MaCombo;
                chiTiet.SoLuongCombo = chiTiet.SoLuongCombo; 
                await _context.ChiTietCombos.AddAsync(chiTiet);
            }

            await _context.SaveChangesAsync();
        }


        public async Task Update(Combo combo, List<ChiTietCombo> chiTietCombos)
        {
            if (combo == null)
                throw new ArgumentNullException(nameof(combo));

            var existingCombo = await _context.Combos.FirstOrDefaultAsync(cb => cb.MaCombo == combo.MaCombo);
            if (existingCombo != null)
            {
                existingCombo.TenCombo = combo.TenCombo;
                existingCombo.Gia = combo.Gia;
                existingCombo.MoTa = combo.MoTa;
                existingCombo.Anh = combo.Anh;
                existingCombo.TrangThai = combo.TrangThai;
                existingCombo.SoLuongCombo = combo.SoLuongCombo;

                var existingDetails = await _context.ChiTietCombos.Where(c => c.MaCombo == combo.MaCombo).ToListAsync();
                _context.ChiTietCombos.RemoveRange(existingDetails);

                foreach (var chiTiet in chiTietCombos)
                {
                    var sanPham = await _context.SanPhams.FirstOrDefaultAsync(sp => sp.MaSanPham == chiTiet.MaSanPham);
                    if (sanPham == null)
                        throw new Exception($"Sản phẩm với ID {chiTiet.MaSanPham} không tồn tại.");

                    chiTiet.MaCombo = combo.MaCombo;
                    chiTiet.SoLuongCombo = chiTiet.SoLuongCombo; // Lưu số lượng sản phẩm trong combo
                    await _context.ChiTietCombos.AddAsync(chiTiet);
                }

                await _context.SaveChangesAsync();
            }
        }


        public async Task Delete(string id)
        {
            var comboToDelete = await _context.Combos.FirstOrDefaultAsync(cb => cb.MaCombo == id);
            if (comboToDelete != null)
            {
                _context.Combos.Remove(comboToDelete);
                await _context.SaveChangesAsync();
            }
        }
    }
}
