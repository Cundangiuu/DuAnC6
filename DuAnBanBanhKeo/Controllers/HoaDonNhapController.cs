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
    public class HoaDonNhapController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public HoaDonNhapController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/HoaDonNhap
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HoaDonNhapDto>>> GetHoaDonNhaps()
        {
            var hoaDonNhaps = await _context.HoaDonNhaps
                .Include(hdn => hdn.ChiTietHoaDonNhaps)
                .Include(hdn => hdn.NhanVien)
                .Include(hdn => hdn.NhaCungCap)
                .ToListAsync();

            return _mapper.Map<List<HoaDonNhapDto>>(hoaDonNhaps);
        }

        // GET: api/HoaDonNhap/5
        [HttpGet("{maHoaDonNhap}")]
        public async Task<ActionResult<HoaDonNhapDto>> GetHoaDonNhap(string maHoaDonNhap)
        {
            var hoaDonNhap = await _context.HoaDonNhaps
                .Include(hdn => hdn.ChiTietHoaDonNhaps)
                .Include(hdn => hdn.NhanVien)
                .Include(hdn => hdn.NhaCungCap)
                .FirstOrDefaultAsync(hdn => hdn.MaHoaDonNhap == maHoaDonNhap);

            if (hoaDonNhap == null)
            {
                return NotFound();
            }

            return _mapper.Map<HoaDonNhapDto>(hoaDonNhap);
        }

        // PUT: api/HoaDonNhap/5
        [HttpPut("{maHoaDonNhap}")]
        public async Task<IActionResult> PutHoaDonNhap(string maHoaDonNhap, HoaDonNhapUpdateDto hoaDonNhapUpdateDto)
        {
            if (maHoaDonNhap != hoaDonNhapUpdateDto.MaHoaDonNhap)
            {
                return BadRequest();
            }

            var hoaDonNhap = await _context.HoaDonNhaps
                .Include(hdn => hdn.ChiTietHoaDonNhaps)
                .FirstOrDefaultAsync(hdn => hdn.MaHoaDonNhap == maHoaDonNhap);

            if (hoaDonNhap == null)
            {
                return NotFound();
            }

            // Validate trạng thái
            if (!IsValidTrangThai(hoaDonNhapUpdateDto.TrangThai))
            {
                return BadRequest("Trạng thái không hợp lệ. Chỉ chấp nhận: 0, 1, 2, 3.");
            }

            hoaDonNhap.TrangThai = hoaDonNhapUpdateDto.TrangThai; // Cập nhật trạng thái

            // **Xử lý chi tiết hóa đơn nhập:**
            // 1. Xóa các chi tiết hóa đơn nhập cũ
            foreach (var chiTiet in hoaDonNhap.ChiTietHoaDonNhaps.ToList())
            {
                _context.ChiTietHoaDonNhaps.Remove(chiTiet);
            }

            // 2. Thêm các chi tiết hóa đơn nhập mới
            foreach (var chiTietDto in hoaDonNhapUpdateDto.ChiTietHoaDonNhaps)
            {
                // Lấy thông tin sản phẩm từ MaSP
                var sanPham = await _context.SanPhams.FindAsync(chiTietDto.MaSP);
                if (sanPham == null)
                {
                    // Xử lý lỗi nếu không tìm thấy sản phẩm
                    return BadRequest($"Không tìm thấy sản phẩm với mã {chiTietDto.MaSP}");
                }

                var chiTiet = new ChiTietHoaDonNhap
                {
                    MaHoaDonNhap = maHoaDonNhap,
                    MaSP = chiTietDto.MaSP,
                    TenSP = sanPham.TenSP,  // Lấy TenSP từ sản phẩm
                    SoLuong = chiTietDto.SoLuong,
                    DonGia = chiTietDto.DonGia
                };
                hoaDonNhap.ChiTietHoaDonNhaps.Add(chiTiet);
            }

            // **Cập nhật tổng tiền (nếu cần):**
            hoaDonNhap.TongTien = hoaDonNhap.ChiTietHoaDonNhaps.Sum(ct => ct.SoLuong * ct.DonGia); // Tính lại tổng tiền

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HoaDonNhapExists(maHoaDonNhap))
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

        // POST: api/HoaDonNhap
        [HttpPost]
        public async Task<ActionResult<HoaDonNhapDto>> PostHoaDonNhap(HoaDonNhapCreateDto hoaDonNhapCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Lấy HoTen, MaNV, TenNCC và MaNCC từ DTO
            string hoTen = hoaDonNhapCreateDto.HoTen;
            string maNV = hoaDonNhapCreateDto.MaNV;
            string tenNCC = hoaDonNhapCreateDto.TenNCC;
            string maNCC = hoaDonNhapCreateDto.MaNCC;

            if (string.IsNullOrEmpty(hoTen))
            {
                return BadRequest("Tên nhân viên không hợp lệ.");
            }

            if (string.IsNullOrEmpty(maNV))
            {
                return BadRequest("Mã nhân viên không hợp lệ.");
            }

            if (string.IsNullOrEmpty(tenNCC))
            {
                return BadRequest("Tên nhà cung cấp không hợp lệ.");
            }

            if (string.IsNullOrEmpty(maNCC))
            {
                return BadRequest("Mã nhà cung cấp không hợp lệ.");
            }

            var hoaDonNhap = new HoaDonNhap
            {
                MaHoaDonNhap = GenerateMaHoaDonNhap(),
                NgayNhap = DateTime.Now,
                MaNV = maNV, // Sử dụng MaNV đã nhận được
                MaNCC = maNCC, // Sử dụng MaNCC đã nhận được
                TongTien = hoaDonNhapCreateDto.TongTien,
                TrangThai = 0 // Đặt trạng thái mặc định
            };

            _context.HoaDonNhaps.Add(hoaDonNhap);

            try
            {
                foreach (var chiTietDto in hoaDonNhapCreateDto.ChiTietHoaDonNhaps)
                {
                    var sanPham = await _context.SanPhams.FindAsync(chiTietDto.MaSP);

                    if (sanPham == null)
                    {
                        return BadRequest($"Không tìm thấy sản phẩm với mã: {chiTietDto.MaSP}");
                    }

                    sanPham.SoLuongTon += chiTietDto.SoLuong; // Cập nhật SoLuongTon ở đây!

                    // Lấy tên sản phẩm từ cơ sở dữ liệu dựa trên MaSP
                    string tenSanPham = sanPham.TenSP;

                    var chiTiet = new ChiTietHoaDonNhap
                    {
                        MaHoaDonNhap = hoaDonNhap.MaHoaDonNhap,
                        MaSP = chiTietDto.MaSP,
                        SoLuong = chiTietDto.SoLuong,
                        DonGia = chiTietDto.DonGia,
                        TenSP = tenSanPham // Gán tên sản phẩm lấy từ cơ sở dữ liệu
                    };
                    _context.ChiTietHoaDonNhaps.Add(chiTiet);
                }

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Ghi log lỗi
                Console.Error.WriteLine(ex);
                return StatusCode(500, "Có lỗi xảy ra khi lưu dữ liệu vào cơ sở dữ liệu.");
            }

            // Load related entities for the DTO (This section might be optimized, see below)
            var hoaDonNhapResult = await _context.HoaDonNhaps
                .Include(h => h.NhaCungCap)
                .Include(h => h.NhanVien)
                .Include(h => h.ChiTietHoaDonNhaps)
                    .ThenInclude(ct => ct.SanPham)
                .FirstOrDefaultAsync(h => h.MaHoaDonNhap == hoaDonNhap.MaHoaDonNhap);

            if (hoaDonNhapResult == null)
            {
                return StatusCode(500, "Không thể tải hóa đơn nhập sau khi tạo.");
            }


            return CreatedAtAction("GetHoaDonNhap", new { maHoaDonNhap = hoaDonNhap.MaHoaDonNhap }, _mapper.Map<HoaDonNhapDto>(hoaDonNhapResult));
        }

        [HttpGet("ExportToWord/{maHoaDonNhap}")]
        public async Task<IActionResult> ExportToWord(string maHoaDonNhap)
        {
            var hoaDonNhap = await _context.HoaDonNhaps
                .Include(hdn => hdn.ChiTietHoaDonNhaps)
                .ThenInclude(ct => ct.SanPham)
                .Include(hdn => hdn.NhanVien)
                .Include(hdn => hdn.NhaCungCap)
                .FirstOrDefaultAsync(hdn => hdn.MaHoaDonNhap == maHoaDonNhap);

            if (hoaDonNhap == null)
            {
                return NotFound();
            }

            try
            {
                byte[] byteArray = GenerateWordDocument(hoaDonNhap);
                return File(byteArray, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", $"HoaDonNhap_{maHoaDonNhap}.docx");
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine(ex.ToString());
                return StatusCode(500, "Lỗi khi tạo file Word.");
            }
        }


        private byte[] GenerateWordDocument(HoaDonNhap hoaDonNhap)
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
                    body.AppendChild(CreateParagraph("HÓA ĐƠN NHẬP", bold: true, center: true));
                    body.AppendChild(CreateParagraph($"Mã hóa đơn: {hoaDonNhap.MaHoaDonNhap}"));
                    body.AppendChild(CreateParagraph($"Ngày nhập: {hoaDonNhap.NgayNhap}"));
                    body.AppendChild(CreateParagraph($"Nhân viên: {hoaDonNhap.NhanVien?.HoTen}"));
                    body.AppendChild(CreateParagraph($"Nhà cung cấp: {hoaDonNhap.NhaCungCap?.TenNCC}"));
                    body.AppendChild(CreateParagraph($"Tổng tiền: {hoaDonNhap.TongTien:N0} VNĐ"));

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
                        ),
                        new TableWidth { Width = "5000", Type = TableWidthUnitValues.Pct } // Set table width to 100% of page width
                    );
                    table.AppendChild<TableProperties>(props);

                    // Define column widths.  Units are twentieths of a point (twips).
                    List<int> columnWidths = new List<int> { 3000, 5000, 2000, 2000 };  //Adjust these values!

                    // Table Grid (defines the columns)
                    TableGrid tg = new TableGrid();
                    foreach (var width in columnWidths)
                    {
                        tg.Append(new GridColumn() { Width = width.ToString() });
                    }
                    table.AppendChild(tg);

                    // Header row
                    TableRow headerRow = new TableRow();
                    headerRow.Append(CreateTableCell("Mã sản phẩm", bold: true, width: columnWidths[0]));
                    headerRow.Append(CreateTableCell("Tên sản phẩm", bold: true, width: columnWidths[1]));
                    headerRow.Append(CreateTableCell("Số lượng", bold: true, width: columnWidths[2]));
                    headerRow.Append(CreateTableCell("Đơn giá", bold: true, width: columnWidths[3]));
                    table.Append(headerRow);


                    // Detail rows
                    foreach (var chiTiet in hoaDonNhap.ChiTietHoaDonNhaps)
                    {
                        TableRow row = new TableRow();
                        row.Append(CreateTableCell(chiTiet.MaSP, width: columnWidths[0]));
                        row.Append(CreateTableCell(chiTiet.TenSP, width: columnWidths[1])); // Sử dụng chiTiet.TenSP
                        row.Append(CreateTableCell(chiTiet.SoLuong.ToString(), width: columnWidths[2]));
                        row.Append(CreateTableCell(chiTiet.DonGia.ToString(), width: columnWidths[3]));
                        table.Append(row);
                    }

                    body.Append(table);


                    wordDocument.MainDocumentPart.Document.Save();
                }
                return mem.ToArray();
            }
        }


        private TableCell CreateTableCell(string text, bool bold = false, int? width = null)
        {
            TableCell cell = new TableCell();

            if (width.HasValue)
            {
                TableCellProperties cellProperties = new TableCellProperties();
                TableCellWidth tableCellWidth = new TableCellWidth() { Width = width.Value.ToString(), Type = TableWidthUnitValues.Dxa }; //Dxa means twentieths of a point
                cellProperties.Append(tableCellWidth);
                cell.Append(cellProperties);
            }


            Paragraph p = new Paragraph();
            ParagraphProperties pp = new ParagraphProperties();
            Justification justification = new Justification() { Val = JustificationValues.Center };
            pp.Append(justification);
            p.AppendChild(pp);


            Run r = new Run();
            if (bold)
            {
                r.Append(new Bold());
            }
            r.Append(new Text(text));
            p.Append(r);
            cell.Append(p);
            return cell;
        }


        private Paragraph CreateParagraph(string text, bool bold = false, bool center = false)
        {
            Paragraph paragraph = new Paragraph();
            ParagraphProperties paragraphProperties = new ParagraphProperties();

            if (center)
            {
                paragraphProperties.Append(new Justification() { Val = JustificationValues.Center });
            }

            paragraph.AppendChild(paragraphProperties);

            Run run = new Run();
            if (bold)
            {
                run.Append(new Bold());
            }
            run.Append(new Text(text) { Space = SpaceProcessingModeValues.Preserve });  // Preserve whitespace.  Important for formatting.
            paragraph.AppendChild(run);

            return paragraph;
        }


        private string GenerateMaHoaDonNhap()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            string maHoaDonNhap;

            do
            {
                maHoaDonNhap = "HDN" + new string(Enumerable.Repeat(chars, 6)
                    .Select(s => s[random.Next(s.Length)]).ToArray());
            }
            while (_context.HoaDonNhaps.Any(hdn => hdn.MaHoaDonNhap == maHoaDonNhap));

            return maHoaDonNhap;
        }

        private bool HoaDonNhapExists(string maHoaDonNhap)
        {
            return _context.HoaDonNhaps.Any(e => e.MaHoaDonNhap == maHoaDonNhap);
        }

        [HttpGet("Products")]
        public async Task<ActionResult<IEnumerable<SanPhamDto>>> GetProducts()
        {
            var sanPhams = await _context.SanPhams.ToListAsync();
            return _mapper.Map<List<SanPhamDto>>(sanPhams);
        }

        [HttpGet("Suppliers")]
        public async Task<ActionResult<IEnumerable<NhaCungCapDto>>> GetSuppliers()
        {
            var nhaCungCaps = await _context.NhaCungCaps.ToListAsync();
            return _mapper.Map<List<NhaCungCapDto>>(nhaCungCaps);
        }

        // Hàm kiểm tra trạng thái hợp lệ
        private bool IsValidTrangThai(int trangThai)
        {
            return trangThai == 0 || trangThai == 1 || trangThai == 2 || trangThai == 3;
        }
    }
}