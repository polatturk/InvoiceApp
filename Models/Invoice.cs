namespace InvoiceApp.Models
{
    public enum OdemeDurumu
    {
        Odendi = 1,
        Bekleniyor = 2,
        HenüzGelmedi = 3
    }

    public class Invoice
    {
        public int Id { get; set; }
        public string Ad { get; set; }
        public string Aciklama { get; set; }
        public DateTime OlusturmaTarihi { get; set; }
        public int Tutar { get; set; }
        public int Toplam { get; set; }
        public string Adet { get; set; }
        public OdemeDurumu OdemeDurumu { get; set; }
        public User User { get; set; }

    }
}
