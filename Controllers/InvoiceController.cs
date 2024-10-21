using InvoiceApp.Data;
using InvoiceApp.Dto;
using InvoiceApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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
        public ActionResult<List<object>> GetInvoices()
        {
            var invoices = _context.Invoices
                .Include(i => i.Customer)
                .Include(i => i.Items)
                .Select(i => new
                {
                    InvoiceId = i.Id,
                    CreatedDate = i.CreatedDate,
                    PaymentStatus = i.PaymentStatus,
                    PaymentTerm = i.PaymentTerm,
                    CustomerName = i.Customer.FullName,
                    CustomerAddress = i.Customer.Address,
                    CustomerEmail = i.Customer.Email,
                    CustomerCity = i.Customer.City,
                    CustomerCountry = i.Customer.Country,
                    CustomerPostCode = i.Customer.PostCode,
                    Items = i.Items.Select(item => new
                    {
                        ItemId = item.Id,
                        Name = item.Name,
                        Description = item.Description,
                        Quantity = item.Quantity,
                        Price = item.Price,
                        Total = item.Total
                    }).ToList(),
                    TotalAmount = i.Items.Sum(item => item.Total)
                })
                .ToList();

            return Ok(invoices);
        }


        [HttpGet("{id}")]
        public ActionResult<Invoice> GetInvoice(int id)
        {
            var invoice = _context.Invoices.FirstOrDefault(i => i.Id == id);

            if (invoice is null)
                return NotFound();

            return Ok(invoice);
        }

        [HttpPost]
        public ActionResult<object> AddInvoice([FromBody] DtoInvoiceCreateRequest invoiceRequest)
        {
            var invoice = new Invoice
            {
                CreatedDate = invoiceRequest.CreatedDate,
                PaymentStatus = invoiceRequest.PaymentStatus,
                PaymentTerm = invoiceRequest.PaymentTerm,
                CustomerId = invoiceRequest.CustomerId,
            };

            var items = _context.Items
                .Where(item => invoiceRequest.ItemIds.Contains(item.Id))
                .ToList();

            invoice.Items = items;

            _context.Invoices.Add(invoice);
            _context.SaveChanges();

            var response = new
            {
                InvoiceId = invoice.Id,
                CreatedDate = invoice.CreatedDate,
                PaymentStatus = invoice.PaymentStatus,
                PaymentTerm = invoice.PaymentTerm,
                CustomerName = _context.Customers
                    .Where(c => c.Id == invoice.CustomerId)
                    .Select(c => c.FullName)
                    .FirstOrDefault(), 
                Items = items.Select(i => new
                {
                    ItemId = i.Id,
                    Name = i.Name,
                    Description = i.Description,
                    Quantity = i.Quantity,
                    Price = i.Price
                }).ToList()
            };

            return CreatedAtAction(nameof(GetInvoice), new { id = invoice.Id }, response);
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
