namespace InvoiceApp.Models
{
    public class Fatura
    {
        public int Id { get; set; }
        public DateTime OlusturmaTarihi { get; set; }
        public string AdSoyad { get; set; }
        public int Tutar { get; set; }
        public bool OdendiMi { get; set; }
    }
}
