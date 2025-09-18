using QuestPdf.Test.Tagging;
using QuestPDF.Fluent;

var invoice = new Invoice
{
    InvoiceNumber = "INV-2025-042",
    IssueDate = DateTime.Now,
    DueDate = DateTime.Now.AddDays(14),
    Seller = new CompanyInfo
    {
        Name = "Example Ltd.",
        Address = "Company Street 1, 12345 Sample City",
        Contact = "+49 123 456789",
        Email = "info@example-ltd.com"
    },
    Customer = new CompanyInfo
    {
        Name = "John Doe",
        Address = "Customer Road 99, 98765 Clienttown",
        Contact = "+49 987 654321",
        Email = "john.doe@mail.com"
    },
    TaxRate = 0.19m,
    PaymentTerms = "Payable within 14 days without deduction.",
    Notes = "Please include the invoice number with your transfer.",
    Items =
    [
        new InvoiceItem { Description = "Web Design Package", Quantity = 1, UnitPrice = 1500m },
        new InvoiceItem { Description = "Maintenance & Support (per month)", Quantity = 3, UnitPrice = 200m },
        new InvoiceItem { Description = "Hosting (12 months)", Quantity = 1, UnitPrice = 120m }
    ]
};

var document = new InvoiceDocument(invoice);
document.GeneratePdf("invoice.pdf");
