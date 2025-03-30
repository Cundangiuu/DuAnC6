using AutoMapper;
using DuAnBanBanhKeo.Data.Entities;
using DuAnBanBanhKeo.Modal;

namespace DuAnBanBanhKeo.Helpers
{
    public class AutoMapperHelper : Profile
    {
        public AutoMapperHelper()
        {
            // TaiKhoan mappings
            CreateMap<TaiKhoan, TaiKhoanDto>()
                .ForMember(dest => dest.HoTen, opt => opt.MapFrom(src => src.NhanVien.HoTen))
                .ForMember(dest => dest.VaiTro, opt => opt.MapFrom(src => src.NhanVien.VaiTro));

            CreateMap<TaiKhoanCreateDto, TaiKhoan>();

            CreateMap<TaiKhoanUpdateDto, TaiKhoan>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null)); // Chỉ map khi giá trị không null

            // HoaDonNhap mappings
            CreateMap<HoaDonNhap, HoaDonNhapDto>()
                .ForMember(dest => dest.HoTen, opt => opt.MapFrom(src => src.NhanVien.HoTen))
                .ForMember(dest => dest.TenNCC, opt => opt.MapFrom(src => src.NhaCungCap.TenNCC))
                .ForMember(dest => dest.ChiTietHoaDonNhaps, opt => opt.MapFrom(src => src.ChiTietHoaDonNhaps));
            CreateMap<HoaDonNhapDto, HoaDonNhap>();

            CreateMap<ChiTietHoaDonNhap, ChiTietHoaDonNhapDto>();
            CreateMap<ChiTietHoaDonNhapDto, ChiTietHoaDonNhap>();

            CreateMap<HoaDonNhapCreateDto, HoaDonNhap>();
            CreateMap<HoaDonNhapUpdateDto, HoaDonNhap>();

            // HoaDonXuat mappings
            CreateMap<HoaDonXuat, HoaDonXuatDto>()
                .ForMember(dest => dest.NhanVien, opt => opt.MapFrom(src => src.NhanVien))
                .ForMember(dest => dest.KhachHang, opt => opt.MapFrom(src => src.KhachHang));
            CreateMap<HoaDonXuatCreateDto, HoaDonXuat>();
            CreateMap<HoaDonXuatUpdateDto, HoaDonXuat>()
                .ForMember(dest => dest.MaHoaDonXuat, opt => opt.Ignore());
            CreateMap<ChiTietHoaDonXuat, ChiTietHoaDonXuatDto>();

            // Other mappings
            CreateMap<NhanVien, NhanVienDto>();
            CreateMap<SanPham, SanPhamDto>();
            CreateMap<NhaCungCap, NhaCungCapDto>();
            CreateMap<KhachHang, KhachHangDto>();
            CreateMap<KhachHangDto, KhachHang>();
        }
    }
}