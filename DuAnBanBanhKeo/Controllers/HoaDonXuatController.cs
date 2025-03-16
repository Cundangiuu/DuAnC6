using AutoMapper;
using DuAnBanBanhKeo.Data.Entities;
using DuAnBanBanhKeo.Data;
using DuAnBanBanhKeo.Modal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml;
using System.IO;
using Paragraph = DocumentFormat.OpenXml.Wordprocessing.Paragraph;
using Run = DocumentFormat.OpenXml.Wordprocessing.Run;
using Text = DocumentFormat.OpenXml.Wordprocessing.Text;

namespace DuAnBanBanhKeo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HoaDonXuatController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public HoaDonXuatController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/HoaDonXuat
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HoaDonXuatDto>>> GetHoaDonXuats()
        {
            var hoaDonXuats = await _context.HoaDonXuats
                .Include(hdx => hdx.ChiTietHoaDonXuat)
                .ThenInclude(ct => ct.SanPham)
                .Include(hdx => hdx.NhanVien)
                .Include(hdx => hdx.KhachHang)
                .ToListAsync();

            return _mapper.Map<List<HoaDonXuatDto>>(hoaDonXuats);
        }

        // GET: api/HoaDonXuat/5
        [HttpGet("{maHoaDonXuat}")]
        public async Task<ActionResult<HoaDonXuatDto>> GetHoaDonXuat(string maHoaDonXuat)
        {
            var hoaDonXuat = await _context.HoaDonXuats
                .Include(hdx => hdx.ChiTietHoaDonXuat)
                .ThenInclude(ct => ct.SanPham)
                .Include(hdx => hdx.NhanVien)
                .Include(hdx => hdx.KhachHang)
                .FirstOrDefaultAsync(hdx => hdx.MaHoaDonXuat == maHoaDonXuat);

            if (hoaDonXuat == null)
            {
                return NotFound();
            }

            return _mapper.Map<HoaDonXuatDto>(hoaDonXuat);
        }

        // PUT: api/HoaDonXuat/5
        [HttpPut("{maHoaDonXuat}")]
        public async Task<IActionResult> PutHoaDonXuat(string maHoaDonXuat, HoaDonXuatUpdateDto hoaDonXuatUpdateDto)
        {
            if (maHoaDonXuat != hoaDonXuatUpdateDto.MaHoaDonXuat)
            {
                return BadRequest();
            }

            var hoaDonXuat = await _context.HoaDonXuats
                .Include(hdx => hdx.ChiTietHoaDonXuat)
                .FirstOrDefaultAsync(hdx => hdx.MaHoaDonXuat == maHoaDonXuat);

            if (hoaDonXuat == null)
            {
                return NotFound();
            }

            _mapper.Map(hoaDonXuatUpdateDto, hoaDonXuat);

            // Validate trạng thái
            if (!IsValidTrangThai(hoaDonXuatUpdateDto.TrangThai))
            {
                return BadRequest("Trạng thái không hợp lệ. Chỉ chấp nhận: 0, 1, 2, 3.");
            }

            hoaDonXuat.TrangThai = hoaDonXuatUpdateDto.TrangThai; // Cập nhật trạng thái

            // Xử lý cập nhật chi tiết hóa đơn
            if (hoaDonXuatUpdateDto.ChiTietHoaDonXuats != null)
            {
                // Lặp qua các chi tiết hóa đơn được gửi đến từ request
                foreach (var chiTietDto in hoaDonXuatUpdateDto.ChiTietHoaDonXuats)
                {
                    var sanPham = await _context.SanPhams.FindAsync(chiTietDto.MaSP);
                    if (sanPham == null)
                    {
                        return BadRequest($"Không tìm thấy sản phẩm với mã {chiTietDto.MaSP}.");
                    }

                    if (sanPham.SoLuongTon == 0)
                    {
                        return BadRequest($"Sản phẩm {sanPham.TenSP} đã hết hàng và không thể thêm/sửa vào hóa đơn.");
                    }
                    //Tìm Chi tiết hóa đơn xuất theo mã sản phẩm
                    var existingChiTiet = hoaDonXuat.ChiTietHoaDonXuat.FirstOrDefault(ct => ct.MaSP == chiTietDto.MaSP);

                    if (existingChiTiet != null)
                    {
                        // Cập nhật số lượng và đơn giá nếu tìm thấy
                        existingChiTiet.SoLuong = chiTietDto.SoLuong;
                        existingChiTiet.DonGia = chiTietDto.DonGia;
                    }
                    else
                    {
                        // Nếu không tìm thấy, tạo mới chi tiết hóa đơn

                        var newChiTiet = new ChiTietHoaDonXuat
                        {
                            MaHoaDonXuat = hoaDonXuat.MaHoaDonXuat,
                            MaSP = chiTietDto.MaSP,
                            SoLuong = chiTietDto.SoLuong,
                            DonGia = chiTietDto.DonGia // You might want to use sanPham.GiaBan here as well, depending on the scenario
                        };
                        _context.ChiTietHoaDonXuats.Add(newChiTiet);
                        hoaDonXuat.ChiTietHoaDonXuat.Add(newChiTiet);
                    }
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HoaDonXuatExists(maHoaDonXuat))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        // POST: api/HoaDonXuat
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<HoaDonXuatDto>> PostHoaDonXuat(HoaDonXuatCreateDto hoaDonXuatCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var maNVClaim = User.Claims.FirstOrDefault(c => c.Type == "MaNV");

            if (maNVClaim == null)
            {
                return BadRequest("Không tìm thấy thông tin nhân viên trong token.");
            }

            string maNV = maNVClaim.Value;

            if (string.IsNullOrEmpty(maNV))
            {
                return BadRequest("Mã nhân viên không hợp lệ.");
            }

            string maHoaDonXuat = GenerateMaHoaDonXuat();

            var hoaDonXuat = new HoaDonXuat
            {
                MaHoaDonXuat = maHoaDonXuat,
                NgayXuat = DateTime.Now,
                MaNV = maNV,
                MaKH = hoaDonXuatCreateDto.MaKH,
                TongTien = hoaDonXuatCreateDto.TongTien,
                TrangThai = 0 // Đặt trạng thái mặc định
            };

            _context.HoaDonXuats.Add(hoaDonXuat);

            foreach (var chiTietDto in hoaDonXuatCreateDto.ChiTietHoaDonXuats)
            {
                var sanPham = await _context.SanPhams.FindAsync(chiTietDto.MaSP);
                if (sanPham == null)
                {
                    return BadRequest($"Không tìm thấy sản phẩm với mã {chiTietDto.MaSP}.");
                }

                // Validate if the product is out of stock
                if (sanPham.SoLuongTon == 0)
                {
                    return BadRequest($"Sản phẩm {sanPham.TenSP} đã hết hàng và không thể thêm vào hóa đơn.");
                }
                // Lấy giá bán từ sản phẩm
                decimal giaBan = sanPham.GiaBan;

                //Giảm số lượng tồn kho khi bán sản phẩm
                sanPham.SoLuongTon -= chiTietDto.SoLuong;
                if (sanPham.SoLuongTon < 0)
                {
                    return BadRequest($"Số lượng sản phẩm {sanPham.TenSP} trong kho không đủ.");
                }

                var chiTiet = new ChiTietHoaDonXuat
                {
                    MaHoaDonXuat = hoaDonXuat.MaHoaDonXuat,
                    MaSP = chiTietDto.MaSP,
                    SoLuong = chiTietDto.SoLuong,
                    DonGia = giaBan // Sử dụng giá bán
                };
                _context.ChiTietHoaDonXuats.Add(chiTiet);
            }

            await _context.SaveChangesAsync();

            // Load related entities for the DTO
            await _context.Entry(hoaDonXuat).Reference(h => h.KhachHang).LoadAsync();
            await _context.Entry(hoaDonXuat).Reference(h => h.NhanVien).LoadAsync();
            foreach (var ct in hoaDonXuat.ChiTietHoaDonXuat)
            {
                await _context.Entry(ct).Reference(c => c.SanPham).LoadAsync();
            }

            return CreatedAtAction("GetHoaDonXuat", new { maHoaDonXuat = hoaDonXuat.MaHoaDonXuat }, _mapper.Map<HoaDonXuatDto>(hoaDonXuat));
        }


        [HttpGet("ExportToWord/{maHoaDonXuat}")]
        public async Task<IActionResult> ExportToWord(string maHoaDonXuat)
        {
            var hoaDonXuat = await _context.HoaDonXuats
                .Include(hdx => hdx.ChiTietHoaDonXuat)
                .ThenInclude(ct => ct.SanPham)
                .Include(hdx => hdx.NhanVien)
                .Include(hdx => hdx.KhachHang)
                .FirstOrDefaultAsync(hdx => hdx.MaHoaDonXuat == maHoaDonXuat);

            if (hoaDonXuat == null)
            {
                return NotFound();
            }

            try
            {
                byte[] byteArray = GenerateWordDocument(hoaDonXuat);
                return File(byteArray, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", $"HoaDonXuat_{maHoaDonXuat}.docx");
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine(ex.ToString());
                return StatusCode(500, "Lỗi khi tạo file Word.");
            }
        }


        private byte[] GenerateWordDocument(HoaDonXuat hoaDonXuat)
        {
            using (MemoryStream mem = new MemoryStream())
            {
                using (WordprocessingDocument wordDocument = WordprocessingDocument.Create(mem, WordprocessingDocumentType.Document))
                {
                    // Create the document structure
                    MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();
                    mainPart.Document = new Document();
                    Body body = mainPart.Document.AppendChild(new Body());

                    // Add title
                    body.AppendChild(CreateParagraph("HÓA ĐƠN XUẤT", bold: true, center: true));
                    body.AppendChild(CreateParagraph($"Mã hóa đơn: {hoaDonXuat.MaHoaDonXuat}"));
                    body.AppendChild(CreateParagraph($"Ngày xuất: {hoaDonXuat.NgayXuat}"));
                    body.AppendChild(CreateParagraph($"Nhân viên: {hoaDonXuat.NhanVien?.HoTen}"));
                    body.AppendChild(CreateParagraph($"Khách hàng: {hoaDonXuat.KhachHang?.TenKH}"));
                    body.AppendChild(CreateParagraph($"Tổng tiền: {hoaDonXuat.TongTien:N0} VNĐ"));

                    // Add table for chi tiết hóa đơn
                    Table table = new Table();

                    // Table properties
                    TableProperties props = new TableProperties(
                        new TableBorders(
                            new TopBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 },
                            new BottomBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 },
                            new LeftBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 },
                            new RightBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 },
                            new InsideHorizontalBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 },
                            new InsideVerticalBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 }
                        )
                    );
                    table.AppendChild<TableProperties>(props);


                    // Header row
                    TableRow headerRow = new TableRow();
                    headerRow.Append(CreateTableCell("Mã SP", bold: true));
                    headerRow.Append(CreateTableCell("Tên SP", bold: true));
                    headerRow.Append(CreateTableCell("Số lượng", bold: true));
                    headerRow.Append(CreateTableCell("Đơn giá", bold: true));
                    table.Append(headerRow);


                    // Detail rows
                    foreach (var chiTiet in hoaDonXuat.ChiTietHoaDonXuat)
                    {
                        TableRow row = new TableRow();
                        row.Append(CreateTableCell(chiTiet.MaSP));
                        row.Append(CreateTableCell(chiTiet.SanPham?.TenSP));
                        row.Append(CreateTableCell(chiTiet.SoLuong.ToString()));
                        row.Append(CreateTableCell(chiTiet.DonGia.ToString()));
                        table.Append(row);
                    }

                    body.Append(table);


                    wordDocument.MainDocumentPart.Document.Save();
                }
                return mem.ToArray();
            }
        }

        private Paragraph CreateParagraph(string text, bool bold = false, bool center = false)
        {
            Paragraph p = new Paragraph();
            ParagraphProperties pPr = new ParagraphProperties();

            if (center)
            {
                Justification justification = new Justification() { Val = JustificationValues.Center };
                pPr.Append(justification);
            }

            Run r = new Run();
            Text t = new Text(text);

            if (bold)
            {
                RunProperties rPr = new RunProperties();
                Bold boldProp = new Bold();
                rPr.Append(boldProp);
                r.Append(rPr);
            }

            r.Append(t);
            p.Append(pPr);
            p.Append(r);

            return p;
        }

        private TableCell CreateTableCell(string text, bool bold = false)
        {
            TableCell cell = new TableCell();
            TableCellProperties cellProperties = new TableCellProperties(
               new TableCellWidth { Type = TableWidthUnitValues.Auto }
            );
            cell.Append(cellProperties);
            cell.Append(CreateParagraph(text, bold: bold));
            return cell;
        }


        private string GenerateMaHoaDonXuat()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            string maHoaDonXuat;

            do
            {
                maHoaDonXuat = "HDX" + new string(Enumerable.Repeat(chars, 6)
                    .Select(s => s[random.Next(s.Length)]).ToArray());
            }
            while (_context.HoaDonXuats.Any(hdx => hdx.MaHoaDonXuat == maHoaDonXuat));

            return maHoaDonXuat;
        }

        private bool HoaDonXuatExists(string maHoaDonXuat)
        {
            return _context.HoaDonXuats.Any(e => e.MaHoaDonXuat == maHoaDonXuat);
        }

        [HttpGet("Products")]
        public async Task<ActionResult<IEnumerable<SanPhamDto>>> GetProducts()
        {
            var sanPhams = await _context.SanPhams.ToListAsync();
            return _mapper.Map<List<SanPhamDto>>(sanPhams);
        }

        [HttpGet("Customers")]
        public async Task<ActionResult<IEnumerable<KhachHangDto>>> GetCustomers()
        {
            var khachHangs = await _context.KhachHangs.ToListAsync();
            return _mapper.Map<List<KhachHangDto>>(khachHangs);
        }

        // Hàm kiểm tra trạng thái hợp lệ
        private bool IsValidTrangThai(int trangThai)
        {
            return trangThai == 0 || trangThai == 1 || trangThai == 2 || trangThai == 3;
        }
    }
}