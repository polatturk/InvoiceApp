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
        public DateTime OlusturmaTarihi { get; set; }
        public OdemeDurumu OdemeDurumu { get; set; }
        public Customer Customers { get; set; }
        public int OdemeVadesi { get; set; }
        public List<Item> Items { get; set; }


    }
}
