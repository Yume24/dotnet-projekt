using CarWorkshopManagementSystem.DTOs.ServiceOrders;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace CarWorkshopManagementSystem.PdfReports;

public class ServiceOrderPdfGenerator
{
    public static byte[] Generate(ServiceOrderDetailsDto order)
    {
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(12));

                page.Header().Text($"Zlecenie serwisowe #{order.Id}").SemiBold().FontSize(20).FontColor(Colors.Blue.Medium);
                
                page.Content().Column(col =>
                {
                    col.Item().Text($"Data utworzenia: {order.CreatedAt:g}");
                    col.Item().Text($"Pojazd: {order.Vehicle}");
                    col.Item().Text($"Mechanik: {order.Mechanic ?? "Nieprzypisany"}");
                    col.Item().Text($"Status: {order.Status}");

                    col.Item().PaddingTop(10).Text("Czynności serwisowe:").Bold();
                    foreach (var task in order.Tasks)
                    {
                        col.Item().Text($"• {task.Description} - {task.LaborCost} PLN");
                        foreach (var part in task.UsedParts)
                        {
                            col.Item().Text($"   ➤ {part.PartName} x{part.Quantity} @ {part.UnitPrice} PLN");
                        }
                    }

                    if (order.Comments.Any())
                    {
                        col.Item().PaddingTop(10).Text("Komentarze:").Bold();
                        foreach (var comment in order.Comments)
                        {
                            col.Item().Text($"[{comment.Timestamp:g}] {comment.Author}: {comment.Content}");
                        }
                    }
                });

                page.Footer().AlignCenter().Text(x =>
                {
                    x.Span("Car Workshop Management System 2.0").FontSize(10);
                });
            });
        });

        return document.GeneratePdf();
    }
}
