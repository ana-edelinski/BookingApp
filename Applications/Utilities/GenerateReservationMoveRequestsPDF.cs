using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Collections.Generic;
using BookingApp.WPF.DTOs;

namespace BookingApp.Applications.Utilities
{
    public static class GenerateReservationMoveRequestsPDF
    {
        public static void GenerateDocument(IEnumerable<AccommodationReservationMoveRequestDTO> requests)
        {
            QuestPDF.Settings.License = LicenseType.Community;
            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(1, QuestPDF.Infrastructure.Unit.Centimetre);
                    page.Header().Text("Reservation Rescheduling Requests").AlignCenter().FontSize(25);

                    page.Content().Column(column =>
                    {
                        column.Spacing(2, Unit.Centimetre);
                        column.Item().Text("Active Reschedule Requests:").AlignCenter().FontSize(20);

                        column.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });

                            table.Header(header =>
                            {
                                header.Cell().Element(CellStyle).Text("Accommodation Name").FontSize(12).Bold().AlignCenter();
                                header.Cell().Element(CellStyle).Text("Start Date").FontSize(12).Bold().AlignCenter();
                                header.Cell().Element(CellStyle).Text("End Date").FontSize(12).Bold().AlignCenter();
                                header.Cell().Element(CellStyle).Text("Status").FontSize(12).Bold().AlignCenter();
                                header.Cell().Element(CellStyle).Text("Owner's Comment").FontSize(12).Bold().AlignCenter();
                            });

                            foreach (var request in requests)
                            {
                                table.Cell().Element(CellStyle).Text(request.AccommodationName).FontSize(12).AlignCenter();
                                table.Cell().Element(CellStyle).Text(request.StartDate.ToString("dd.MM.yyyy")).FontSize(12).AlignCenter();
                                table.Cell().Element(CellStyle).Text(request.EndDate.ToString("dd.MM.yyyy")).FontSize(12).AlignCenter();
                                table.Cell().Element(CellStyle).Text(request.Status).FontSize(12);
                                table.Cell().Element(CellStyle).Text(request.CommentText).FontSize(12).AlignCenter();
                            }
                        });
                    });
                });
            }).GeneratePdfAndShow();
        }

        private static IContainer CellStyle(IContainer container)
        {
            return container.PaddingVertical(1).PaddingHorizontal(5);
        }
    }
}