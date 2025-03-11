using DuAnBanBanhKeo.Data.Entities;

namespace DuAnBanBanhKeo.Responsive
{
    public interface IGiamGiaServices
    {
        Task<IEnumerable<GiamGia>> GetAllGiamGia();
        Task<GiamGia> GetGiamGiaById(string id);
        Task AddGiamGia(GiamGia giamGia);
        Task UpdateGiamGia(GiamGia giamGia);
        Task DeleteGiamGia(string id);

        Task<IEnumerable<MaNhapGiamGia>> GetMaNhapGiamGiaByGiamGiaId(string maGiamGia);
        Task AddMaNhapGiamGia(string maGiamGia);
        Task MarkMaNhapGiamGiaAsUsed(int id);
    }

}
