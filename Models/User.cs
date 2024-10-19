namespace InvoiceApp.Models
{
    public class User
    {
        public int Id { get; set; }
        public string AdSoyad { get; set; }
        public string Eposta { get; set; }
        public string Adres { get; set; }
        public string City { get; set; }
        public int Kod { get; set; }
        public string Ulke { get; set; }
        public List<Invoice> Invoices { get; set; }

    }
}
