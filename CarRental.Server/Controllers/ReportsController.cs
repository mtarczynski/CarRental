using CarRental.Server.Entities;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using QuestPDF.Helpers;
using Rotativa.AspNetCore;
using Colors = QuestPDF.Helpers.Colors;

namespace CarRental.Server.Controllers
{
    [ApiController]
    [Route("api/reports")]
    public class ReportsController : ControllerBase
    {
        private readonly CarRentalDbContext _context;

        public ReportsController(CarRentalDbContext context)
        {
            _context = context;
        }

        [HttpGet("fleet/excel")]
        public async Task<IActionResult> GetFleetReportExcel()
        {
            int totalCustomers = await _context.Customers.CountAsync();
            int totalVehicles = await _context.Vehicles.CountAsync();
            int availableVehicles = await _context.Vehicles
                .Where(v => !v.Reservations.Any(r => r.Status == "Active"))
                .CountAsync();
            int reservedVehicles = totalVehicles - availableVehicles;

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Raport floty");

                worksheet.Cell(1, 1).Value = "Wszystkich samochodów";
                worksheet.Cell(1, 2).Value = "Dostępnych samochodów";
                worksheet.Cell(1, 3).Value = "Zarezerwowanych samochodów";
                worksheet.Cell(1, 4).Value = "Klientów łącznie";

                worksheet.Cell(2, 1).Value = totalVehicles;
                worksheet.Cell(2, 2).Value = availableVehicles;
                worksheet.Cell(2, 3).Value = reservedVehicles;
                worksheet.Cell(2, 4).Value = totalCustomers;


                worksheet.Range("A1:D1").Style.Font.Bold = true;
                var tableRange = worksheet.Range("A1:D2");
                tableRange.Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
                tableRange.Style.Border.InsideBorder = XLBorderStyleValues.Medium;
                worksheet.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "FleetReport.xlsx");
                }

            }
        }

        [HttpGet("fleet/pdf")]
        public async Task<IActionResult> GetFleetReportPdf()
        {
            int totalCustomers = await _context.Customers.CountAsync();
            int totalVehicles = await _context.Vehicles.CountAsync();
            int availableVehicles = await _context.Vehicles
                .Where(v => !v.Reservations.Any(r => r.Status == "Active"))
                .CountAsync();
            int reservedVehicles = totalVehicles - availableVehicles;

            byte[] pdfBytes = GenerateFleetReportPdf(totalVehicles, availableVehicles, reservedVehicles, totalCustomers);

            return File(pdfBytes, "application/pdf", "FleetReport.pdf");
        }

        private byte[] GenerateFleetReportPdf(int totalVehicles, int availableVehicles, int reservedVehicles, int totalCustomers)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            using (var stream = new MemoryStream())
            {
                Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Margin(30);
                        page.Header()
                            .Text("Raport Floty Samochodów")
                            .SemiBold().FontSize(20).FontColor(QuestPDF.Helpers.Colors.Blue.Medium);

                        page.Content()
                            .Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.ConstantColumn(150);
                                    columns.RelativeColumn();
                                });

                                table.Header(header =>
                                {
                                    header.Cell().Element(CellStyle).Text("Kategoria").Bold();
                                    header.Cell().Element(CellStyle).Text("Wartość").Bold();
                                });

                                table.Cell().Element(CellStyle).Text("Wszystkich samochodów:");
                                table.Cell().Element(CellStyle).Text(totalVehicles.ToString());

                                table.Cell().Element(CellStyle).Text("Dostępnych samochodów:");
                                table.Cell().Element(CellStyle).Text(availableVehicles.ToString());

                                table.Cell().Element(CellStyle).Text("Zarezerwowanych samochodów:");
                                table.Cell().Element(CellStyle).Text(reservedVehicles.ToString());

                                table.Cell().Element(CellStyle).Text("Klientów łącznie:");
                                table.Cell().Element(CellStyle).Text(totalCustomers.ToString());

                                static IContainer CellStyle(IContainer container) =>
                                    container.PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Grey.Lighten2);
                            });

                        page.Footer()
                            .AlignCenter()
                            .Text($"Raport wygenerowany: {System.DateTime.Now:yyyy-MM-dd HH:mm}");
                    });
                }).GeneratePdf(stream);

                return stream.ToArray();
            }
        }
    }
}
