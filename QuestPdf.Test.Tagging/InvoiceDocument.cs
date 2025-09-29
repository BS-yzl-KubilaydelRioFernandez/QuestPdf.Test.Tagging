using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace QuestPdf.Test.Tagging
{
    internal class InvoiceDocument : IDocument
    {
        private readonly Invoice _model;

        public InvoiceDocument(Invoice model)
        {
            _model = model;

            QuestPDF.Settings.License = LicenseType.Community;
        }

        public DocumentMetadata GetMetadata() => new() { Title = "Invoice Example", Language = "en-US" };

        public DocumentSettings GetSettings() => new() { PDFA_Conformance = PDFA_Conformance.PDFA_3A, PDFUA_Conformance = PDFUA_Conformance.PDFUA_1 };

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Margin(40);

                // Header
                page.Header().Row(row =>
                {
                    row.RelativeItem().Column(column =>
                    {
                        column.Item().SemanticHeader1().Text(_model.Seller.Name).FontSize(20).SemiBold();   // headers do have alternate text set automatically, which produces a quality warning (see https://pac.pdf-accessibility.org/en/resources/pac-2024-quality-checks/alternative-text-on-text-elements)
                        column.Item().SemanticParagraph().Text(_model.Seller.Address);
                        column.Item().SemanticParagraph().Text($"Phone: {_model.Seller.Contact}");
                        column.Item().SemanticParagraph().SemanticLink("Email").Text(_model.Seller.Email);
                    });

                    row.ConstantItem(100).ArtifactOther().Height(60)
                        .Background(Colors.Grey.Lighten3).AlignCenter().AlignMiddle()
                        .Text("LOGO").FontSize(14);
                });

                // Content
                page.Content().PaddingVertical(20).Column(column =>
                {
                    // Invoice Info
                    column.Item().Row(row =>
                    {
                        row.RelativeItem().SemanticSection().Column(column =>
                        {
                            // Maybe automatic tagging for Text or new element in API?
                            column.Item().SemanticParagraph().Text("Invoice to:").SemiBold();
                            column.Item().SemanticParagraph().Text(_model.Customer.Name);
                            column.Item().SemanticParagraph().Text(_model.Customer.Address);
                            column.Item().SemanticParagraph().Text(_model.Customer.Contact);
                            column.Item().SemanticParagraph().SemanticLink("Email").Text(_model.Customer.Email);
                        });

                        row.RelativeItem().SemanticSection().Column(column =>
                        {
                            column.Item().SemanticParagraph().Text($"Invoice: {_model.InvoiceNumber}").SemiBold();
                            column.Item().SemanticParagraph().Text($"Date: {_model.IssueDate:dd.MM.yyyy}");
                            column.Item().SemanticParagraph().Text($"Due by: {_model.DueDate:dd.MM.yyyy}");
                        });
                    });

                    column.Item().PaddingVertical(10).LineHorizontal(1);    // automatically adds layout artifact (very nice)

                    // Table
                    column.Item().Table(table =>
                    {
                        table.ApplySemanticTags();

                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(6);
                            columns.RelativeColumn(2);
                            columns.RelativeColumn(2);
                            columns.RelativeColumn(2);
                        });

                        table.Header(header =>
                        {
                            header.Cell().Text("Description").SemiBold();
                            header.Cell().AlignRight().Text("Quantity").SemiBold();
                            header.Cell().AlignRight().Text("Unit Price").SemiBold();
                            header.Cell().AlignRight().Text("Total").SemiBold();
                        });

                        foreach (var item in _model.Items)
                        {
                            table.Cell().Text(item.Description);
                            table.Cell().Text(item.Quantity.ToString());
                            table.Cell().Text($"{item.UnitPrice:C}");
                            table.Cell().Text($"{item.Total:C}");
                        }
                    });

                    column.Item().PaddingVertical(10).LineHorizontal(1);

                    // Totals
                    var subtotal = 0m;
                    foreach (var item in _model.Items)
                    {
                        subtotal += item.Total;
                    }

                    var tax = subtotal * _model.TaxRate;
                    var total = subtotal + tax;

                    column.Item().SemanticSection().AlignRight().Column(column =>
                    {
                        column.Spacing(2);
                        column.Item().SemanticParagraph().Text($"Subtotal: {subtotal:C}");
                        column.Item().SemanticParagraph().Text($"VAT ({_model.TaxRate:P0}): {tax:C}");
                        column.Item().SemanticParagraph().Text($"Total Amount: {total:C}").Bold().FontSize(14);
                    });

                    // Notes
                    if (!string.IsNullOrWhiteSpace(_model.PaymentTerms) || !string.IsNullOrWhiteSpace(_model.Notes))
                    {
                        column.Item().SemanticParagraph().PaddingTop(20).Column(column =>
                        {
                            if (!string.IsNullOrWhiteSpace(_model.PaymentTerms))
                            {
                                column.Item().Text($"Payment Terms: {_model.PaymentTerms}");
                            }

                            if (!string.IsNullOrWhiteSpace(_model.Notes))
                            {
                                column.Item().Text($"Notes: {_model.Notes}");
                            }
                        });
                    }
                });

                // Footer
                page.Footer().SemanticParagraph().AlignCenter().Text(text =>
                {
                    text.Span("Thank you for your trust!").Italic();
                });
            });
        }
    }
}
