using CarWorkshopManagementSystem.DTOs.ServiceOrders;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace CarWorkshopManagementSystem.PdfReports;

public class AllOrdersPdfGenerator
{
    public static byte[] Generate(List<ServiceOrderDetailsDto> orders)
    {
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.DefaultTextStyle(x => x.FontSize(11));

                page.Header().Text("Lista wszystkich zleceń serwisowych")
                             .SemiBold().FontSize(18).FontColor(Colors.Blue.Medium);

                page.Content().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.ConstantColumn(40);   // ID
                        columns.RelativeColumn();     // Pojazd
                        columns.RelativeColumn();     // Mechanik
                        columns.RelativeColumn();     // Status
                        columns.RelativeColumn();     // Data
                    });

                    // Nagłówki
                    table.Header(header =>
                    {
                        header.Cell().Element(CellStyle).Text("ID").Bold();
                        header.Cell().Element(CellStyle).Text("Pojazd").Bold();
                        header.Cell().Element(CellStyle).Text("Mechanik").Bold();
                        header.Cell().Element(CellStyle).Text("Status").Bold();
                        header.Cell().Element(CellStyle).Text("Utworzono").Bold();
                    });

                    // Dane
                    foreach (var order in orders)
                    {
                        table.Cell().Element(CellStyle).Text(order.Id.ToString());
                        table.Cell().Element(CellStyle).Text(order.Vehicle);
                        table.Cell().Element(CellStyle).Text(order.Mechanic ?? "-");
                        table.Cell().Element(CellStyle).Text(order.Status);
                        table.Cell().Element(CellStyle).Text(order.CreatedAt.ToString("g"));
                    }

                    IContainer CellStyle(IContainer container)
                    {
                        return container
                            .PaddingVertical(4)
                            .PaddingHorizontal(2);
                    }
                });

                page.Footer().AlignCenter().Text(x => x.Span($"Wygenerowano: {DateTime.Now:g}").FontSize(10));
            });
        });

        return document.GeneratePdf();
    }
}
