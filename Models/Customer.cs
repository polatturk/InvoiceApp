namespace InvoiceApp.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string AdSoyad { get; set; }
        public string EPosta { get; set; }
        public string Adres { get; set; }
        public string Sehir { get; set; }
        public string Ulke { get; set; }
        public int kod { get; set; }
        public User Users { get; set; }

    }
}
