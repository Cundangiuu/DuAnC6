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

            CreateMap<HoaDonXuat, HoaDonXuatDto>()
                .ForMember(dest => dest.NhanVien, opt => opt.MapFrom(src => src.NhanVien))
                .ForMember(dest => dest.KhachHang, opt => opt.MapFrom(src => src.KhachHang)); // Map KhachHang from HoaDonXuat to HoaDonXuatDto
            CreateMap<HoaDonXuatCreateDto, HoaDonXuat>();
            CreateMap<HoaDonXuatUpdateDto, HoaDonXuat>()
                .ForMember(dest => dest.MaHoaDonXuat, opt => opt.Ignore());
            CreateMap<ChiTietHoaDonXuat, ChiTietHoaDonXuatDto>();

            CreateMap<NhanVien, NhanVienDto>(); // Add this mapping
            CreateMap<SanPham, SanPhamDto>();
            CreateMap<NhaCungCap, NhaCungCapDto>();
            CreateMap<KhachHang, KhachHangDto>(); // Add this mapping
            CreateMap<KhachHangDto, KhachHang>(); // Optional: Add this if you need to map back from DTO to entity.

        }
    }
}