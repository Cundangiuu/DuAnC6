using AutoMapper;
using DuAnBanBanhKeo.Data.Entities;
using DuAnBanBanhKeo.Modal;

namespace DuAnBanBanhKeo.Helpers
{
    public class AutoMapperHelper : Profile
    {
        public AutoMapperHelper()
        {
            CreateMap<TaiKhoan, TaiKhoanDto>();
            CreateMap<TaiKhoanCreateDto, TaiKhoan>();
            CreateMap<TaiKhoanUpdateDto, TaiKhoan>();


            CreateMap<HoaDonNhap, HoaDonNhapDto>()
                .ForMember(dest => dest.HoTen, opt => opt.MapFrom(src => src.NhanVien.HoTen))
                .ForMember(dest => dest.TenNCC, opt => opt.MapFrom(src => src.NhaCungCap.TenNCC))
                .ForMember(dest => dest.ChiTietHoaDonNhaps, opt => opt.MapFrom(src => src.ChiTietHoaDonNhaps));
            CreateMap<HoaDonNhapDto, HoaDonNhap>();

            CreateMap<ChiTietHoaDonNhap, ChiTietHoaDonNhapDto>(); 
            CreateMap<ChiTietHoaDonNhapDto, ChiTietHoaDonNhap>();

            CreateMap<HoaDonNhapCreateDto, HoaDonNhap>(); 
            CreateMap<HoaDonNhapUpdateDto, HoaDonNhap>();

            CreateMap<SanPham, SanPhamDto>();
            CreateMap<NhaCungCap, NhaCungCapDto>();
        }
    }
}
