using InvoiceApp.Data;
using InvoiceApp.Dto;
using InvoiceApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceApp.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class InvoiceController : ControllerBase
    {
        private readonly AppDbContext _context;

        public InvoiceController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public List<Invoice> GetInvoices()
        {
            List<Invoice> invoices = _context.Invoices.ToList();
            return invoices;
        }

        [HttpGet("{id}")]
        public ActionResult<Invoice> GetInvoice(int id)
        {
            var invoice = _context.Invoices.FirstOrDefault();

            if (invoice is null)
                return NotFound();

            return Ok(invoice);
        }

        [HttpPost]
        public ActionResult<Invoice> AddInvoice([FromBody] DtoInvoiceCreateRequest invoicerequest)
        {
            var invoice = new Invoice
            {
                CreatedDate = invoicerequest.CreatedDate,
                PaymentStatus = invoicerequest.PaymentStatus,
                PaymentTerm = invoicerequest.PaymentTerm,
                CustomerId = invoicerequest.CustomerId,
            };

            _context.Invoices.Add(invoice);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetInvoice), new { id = invoice.Id }, invoice);
        }

        [HttpPut("{id}")]
        public ActionResult<Invoice> UpdateInvoice(int id, [FromBody] DtoInvoiceUpdateRequest invoiceRequest)
        {
            var invoice = _context.Invoices.Find(id);
            if (invoice is null)
                return NotFound();

            invoice.CreatedDate = invoiceRequest.CreatedDate;
            invoice.PaymentStatus = invoiceRequest.PaymentStatus;
            invoice.PaymentTerm = invoiceRequest.PaymentTerm;
            invoice.CustomerId = invoiceRequest.CustomerId;

            _context.SaveChanges();

            return Ok(invoice);
        }

        [HttpDelete("{id}")]
        public bool DeleteInvoice(int id)
        {
            var invoice = _context.Invoices.Find(id);
            if (invoice is null)
                return false;
            _context.Invoices.Remove(invoice);
            _context.SaveChanges();
            return true;
        }
    }
}
