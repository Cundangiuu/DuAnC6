using DuAnBanBanhKeo.Data;
using DuAnBanBanhKeo.Data.Entities;
using DuAnBanBanhKeo.Responsive;
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

            // Tạo mã combo tự động với tiền tố "MCB" và 8 ký tự ngẫu nhiên
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random random = new Random();
            string randomCode = new string(Enumerable.Repeat(chars, 8)
                .Select(s => s[random.Next(s.Length)])
                .ToArray());
            combo.MaCombo = "MCB" + randomCode; // Kết hợp tiền tố "MCB" và mã ngẫu nhiên

            // Thêm combo vào cơ sở dữ liệu
            await _context.Combos.AddAsync(combo);
            await _context.SaveChangesAsync();

            // Thêm các chi tiết sản phẩm vào ChiTietCombo
            foreach (var chiTiet in chiTietCombos)
            {
                // Lấy sản phẩm từ MaSanPham trong ChiTietCombo
                var sanPham = await _context.SanPhams.FirstOrDefaultAsync(sp => sp.MaSanPham == chiTiet.MaSanPham);
                if (sanPham == null)
                    throw new Exception($"Sản phẩm với ID {chiTiet.MaSanPham} không tồn tại.");

                // Cập nhật lại thông tin ChiTietCombo
                chiTiet.MaCombo = combo.MaCombo;
                chiTiet.SoLuongCombo = chiTiet.SoLuongCombo; // Lưu số lượng sản phẩm trong combo
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

                // Xóa các sản phẩm hiện tại trong combo
                var existingDetails = await _context.ChiTietCombos.Where(c => c.MaCombo == combo.MaCombo).ToListAsync();
                _context.ChiTietCombos.RemoveRange(existingDetails);

                // Thêm lại các sản phẩm mới vào Combo
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
