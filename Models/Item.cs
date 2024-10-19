namespace InvoiceApp.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Ad { get; set; }
        public string Aciklama { get; set; }
        public int Adet { get; set; }
        public double Tutar { get; set; }
        public double Toplam { get; set; }
        public Invoice Invoices { get; set; }

    }
}
