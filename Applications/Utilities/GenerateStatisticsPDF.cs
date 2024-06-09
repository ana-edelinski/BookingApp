using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Collections.Generic;

namespace BookingApp.Applications.Utilities
{
    public static class GenerateStatisticsPDF
    {
        public static void GenerateDocument(IEnumerable<dynamic> statistics, string selectedOption, string accommodationName)
        {
            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, QuestPDF.Infrastructure.Unit.Centimetre);
                    page.Header().Text("Accommodation Statistics").AlignCenter().FontSize(25);

                    page.Content().Column(column => {


                        column.Spacing(1, Unit.Centimetre);
                        column.Item().Text("Accommodation name:" + accommodationName);
                        column.Item().Text("Option: " +  selectedOption);
                        column.Item().Table(table =>
                        {

                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });

                            Generate(statistics, selectedOption, table);
                        });


                    });

                });
            }).GeneratePdfAndShow();
        }

        private static void Generate(IEnumerable<dynamic> statistics, string selectedOption, TableDescriptor table)
        {
            if (selectedOption == "All years")
            {
                GenerateTable(statistics, table, "Year");
            }
            else
            {
                GenerateTable(statistics, table, "Month");
            }

        }

        private static void GenerateTable(IEnumerable<dynamic> statistics, TableDescriptor table, string timeLabel)
        {
            table.Cell().Text(timeLabel);
            table.Cell().Text("Reservations");
            table.Cell().Text("Cancelations");
            table.Cell().Text("Reschedulings");
            table.Cell().Text("Recommendations");
            table.Cell().Text("Busyness");
            foreach (var stat in statistics)
            {
                int reservations = stat.Reservations;
                int cancelations = stat.Cancelations;
                int reschedulings = stat.Reschedulings;
                int recommendations = stat.Recommendations;
                int busyness = stat.Busyness;
                string time = stat.Time;

                table.Cell().Text(time.ToString());
                table.Cell().Text(reservations.ToString());
                table.Cell().Text(cancelations.ToString());
                table.Cell().Text(reschedulings.ToString());
                table.Cell().Text(recommendations.ToString());
                table.Cell().Text(busyness.ToString());
            }
        }

    }
}
