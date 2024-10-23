using InvoiceApp.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoiceApp.Dto
{
    public class DtoInvoiceCreateRequest
    {
        public DateTime CreatedDate { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public int PaymentTerm { get; set; }
        public int CustomerId { get; set; }
        public string Description { get; set; }
        public List<int> ItemIds { get; set; } = new List<int>();
        public List<int> Quantities { get; set; } = new List<int>();
    }
    public class DtoInvoiceUpdateRequest
    {
        public int Id { get; set; }  
        public DateTime CreatedDate { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public string Description { get; set; }
        public int PaymentTerm { get; set; }
        public int CustomerId { get; set; }
    }
}
