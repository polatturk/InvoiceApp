using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace InvoiceApp.Models
{
    public enum PaymentStatus
    {
        Odendi = 1,
        Bekleniyor = 2,
        HenüzGelmedi = 3
    }

    public class Invoice
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public Customer Customer { get; set; }
        public int PaymentTerm { get; set; }
        public List<Item> Items { get; set; }
    }
}
