namespace QuestPdf.Test.Tagging
{
    internal class Invoice
    {
        public string InvoiceNumber { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }

        public CompanyInfo Seller { get; set; }
        public CompanyInfo Customer { get; set; }

        public List<InvoiceItem> Items { get; set; }
        public decimal TaxRate { get; set; }
        public string PaymentTerms { get; set; }
        public string Notes { get; set; }
    }
}
