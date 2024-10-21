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
        public DateTime UpdatedDate { get; set; } = DateTime.Now;
        public PaymentStatus PaymentStatus { get; set; }
        public int CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }
        public int PaymentTerm { get; set; }
        public string Description { get; set; }
        public ICollection<Item> Items { get; set; } = new List<Item>();
        public double TotalAmount => Items.Sum(item => item.Total);
    }
}
                         