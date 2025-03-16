﻿using AutoMapper;
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
                .ThenInclude(ct => ct.SanPham)
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
                .ThenInclude(ct => ct.SanPham)
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

            _mapper.Map(hoaDonNhapUpdateDto, hoaDonNhap);

            // Validate trạng thái
            if (!IsValidTrangThai(hoaDonNhapUpdateDto.TrangThai))
            {
                return BadRequest("Trạng thái không hợp lệ. Chỉ chấp nhận: 0, 1, 2, 3.");
            }

            hoaDonNhap.TrangThai = hoaDonNhapUpdateDto.TrangThai; // Cập nhật trạng thái

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
        [Authorize]
        public async Task<ActionResult<HoaDonNhapDto>> PostHoaDonNhap(HoaDonNhapCreateDto hoaDonNhapCreateDto)
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

            string maHoaDonNhap = GenerateMaHoaDonNhap();

            var hoaDonNhap = new HoaDonNhap
            {
                MaHoaDonNhap = maHoaDonNhap,
                NgayNhap = DateTime.Now,
                MaNV = maNV,
                MaNCC = hoaDonNhapCreateDto.MaNCC,
                TongTien = hoaDonNhapCreateDto.TongTien,
                TrangThai = 0 // Đặt trạng thái mặc định
            };

            _context.HoaDonNhaps.Add(hoaDonNhap);

            foreach (var chiTietDto in hoaDonNhapCreateDto.ChiTietHoaDonNhaps)
            {
                var sanPham = await _context.SanPhams.FindAsync(chiTietDto.MaSP);
                if (sanPham != null)
                {
                    sanPham.SoLuongTon += chiTietDto.SoLuong;
                }

                var chiTiet = new ChiTietHoaDonNhap
                {
                    MaHoaDonNhap = hoaDonNhap.MaHoaDonNhap,
                    MaSP = chiTietDto.MaSP,
                    SoLuong = chiTietDto.SoLuong,
                    DonGia = chiTietDto.DonGia
                };
                _context.ChiTietHoaDonNhaps.Add(chiTiet);
            }

            await _context.SaveChangesAsync();

            // Load related entities for the DTO
            await _context.Entry(hoaDonNhap).Reference(h => h.NhaCungCap).LoadAsync();
            await _context.Entry(hoaDonNhap).Reference(h => h.NhanVien).LoadAsync();
            foreach (var ct in hoaDonNhap.ChiTietHoaDonNhaps)
            {
                await _context.Entry(ct).Reference(c => c.SanPham).LoadAsync();
            }

            return CreatedAtAction("GetHoaDonNhap", new { maHoaDonNhap = hoaDonNhap.MaHoaDonNhap }, _mapper.Map<HoaDonNhapDto>(hoaDonNhap));
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
                    foreach (var chiTiet in hoaDonNhap.ChiTietHoaDonNhaps)
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