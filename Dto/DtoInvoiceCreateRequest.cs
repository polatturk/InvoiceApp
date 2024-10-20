using InvoiceApp.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoiceApp.Dto
{
    public class DtoInvoiceCreateRequest
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public int PaymentTerm { get; set; }
        public int CustomerId { get; set; }
    }
}
