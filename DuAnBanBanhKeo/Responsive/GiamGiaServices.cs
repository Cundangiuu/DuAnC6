using DuAnBanBanhKeo.Data.Entities;
using DuAnBanBanhKeo.Data;
using Microsoft.EntityFrameworkCore;

namespace DuAnBanBanhKeo.Responsive
{
    public class GiamGiaResponsive : IGiamGiaServices
    {
        private readonly ApplicationDbContext _context;

        public GiamGiaResponsive(ApplicationDbContext context)
        {
            _context = context;
        }

        // Lấy tất cả giảm giá
        public async Task<IEnumerable<GiamGia>> GetAllGiamGia()
        {
            return await _context.GiamGias.ToListAsync();
        }

        // Lấy giảm giá theo mã
        public async Task<GiamGia> GetGiamGiaById(string id)
        {
            return await _context.GiamGias.FirstOrDefaultAsync(gg => gg.MaGiamGia == id);
        }

        // Thêm mới giảm giá
        public async Task AddGiamGia(GiamGia giamGia)
        {
            if (giamGia == null)
                throw new ArgumentNullException(nameof(giamGia));

            // Tạo mã giảm giá tự động
            giamGia.MaGiamGia = GenerateMaGiamGia();

            await _context.GiamGias.AddAsync(giamGia);
            await _context.SaveChangesAsync();

            // Tạo các mã nhập giảm giá tự động
            for (int i = 0; i < giamGia.SoLuongMaNhapToiDa; i++)
            {
                await AddMaNhapGiamGia(giamGia.MaGiamGia);
            }
        }

        // Cập nhật giảm giá
        public async Task UpdateGiamGia(GiamGia giamGia)
        {
            var existingGiamGia = await _context.GiamGias.FirstOrDefaultAsync(gg => gg.MaGiamGia == giamGia.MaGiamGia);
            if (existingGiamGia != null)
            {
                existingGiamGia.TenGiamGia = giamGia.TenGiamGia;
                existingGiamGia.GiaTri = giamGia.GiaTri;
                existingGiamGia.NgayBatDau = giamGia.NgayBatDau;
                existingGiamGia.NgayKetThuc = giamGia.NgayKetThuc;
                existingGiamGia.SoLuongMaNhapToiDa = giamGia.SoLuongMaNhapToiDa;
                existingGiamGia.TrangThai = giamGia.TrangThai;

                await _context.SaveChangesAsync();
            }
        }

        // Xóa giảm giá
        public async Task DeleteGiamGia(string id)
        {
            var giamGia = await _context.GiamGias.FirstOrDefaultAsync(gg => gg.MaGiamGia == id);
            if (giamGia != null)
            {
                _context.GiamGias.Remove(giamGia);
                await _context.SaveChangesAsync();
            }
        }

        // Lấy mã nhập giảm giá theo mã giảm giá
        public async Task<IEnumerable<MaNhapGiamGia>> GetMaNhapGiamGiaByGiamGiaId(string maGiamGia)
        {
            return await _context.MaNhapGiamGias
                .Where(m => m.MaGiamGia == maGiamGia && !m.IsUsed)
                .ToListAsync();
        }

        // Thêm mã nhập giảm giá tự động
        public async Task AddMaNhapGiamGia(string maGiamGia)
        {
            var maNhap = GenerateMaNhap();
            var maNhapGiamGia = new MaNhapGiamGia
            {
                MaNhap = maNhap,
                MaGiamGia = maGiamGia,
                IsUsed = false
            };

            await _context.MaNhapGiamGias.AddAsync(maNhapGiamGia);
            await _context.SaveChangesAsync();
        }

        // Đánh dấu mã nhập giảm giá là đã sử dụng
        public async Task MarkMaNhapGiamGiaAsUsed(int id)
        {
            var maNhapGiamGia = await _context.MaNhapGiamGias.FirstOrDefaultAsync(m => m.Id == id);
            if (maNhapGiamGia != null)
            {
                maNhapGiamGia.IsUsed = true;
                await _context.SaveChangesAsync();
            }
        }

        // Hàm tạo mã giảm giá ngẫu nhiên
        private string GenerateMaGiamGia()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random random = new Random();
            string randomCode = new string(Enumerable.Repeat(chars, 8)
                .Select(s => s[random.Next(s.Length)])
                .ToArray());
            return "GG" + randomCode; // Kết hợp tiền tố "GG" và mã ngẫu nhiên
        }

        // Hàm tạo mã nhập giảm giá ngẫu nhiên
        private string GenerateMaNhap()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random random = new Random();
            string randomCode = new string(Enumerable.Repeat(chars, 8)
                .Select(s => s[random.Next(s.Length)])
                .ToArray());
            return "MN" + randomCode; // Kết hợp tiền tố "MN" và mã ngẫu nhiên
        }
    }

}
