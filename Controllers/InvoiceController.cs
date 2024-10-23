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
                .Include(i => i.Items)
                .Include(i => i.Client)
                .Select(i => new
                {
                    InvoiceId = i.Id,
                    CreatedDate = i.CreatedDate,
                    PaymentStatus = i.PaymentStatus,
                    PaymentTerm = i.PaymentTerm,
                    Description = i.Description,
                    CustomerName = i.Customer.FullName,
                    CustomerAddress = i.Customer.Address,
                    CustomerEmail = i.Customer.Email,
                    CustomerCity = i.Customer.City,
                    CustomerCountry = i.Customer.Country,
                    CustomerPostCode = i.Customer.PostCode,
                    Client = new
                    {
                        Name = i.Client.Name,
                        City = i.Client.City,
                        Country = i.Client.Country,
                        PostCode = i.Client.PostCode,
                        Address = i.Client.Adress
                    },
                    Items = i.Items.Select(item => new
                    {
                        ItemId = item.Id,
                        Name = item.Name,
                        Description = item.Description,
                        Quantity = item.Quantity,
                        Price = item.Price,
                        Total = item.Price * item.Quantity
                    }).ToList(),
                    TotalAmount = i.Items.Sum(item => item.Price * item.Quantity)
                })
                .ToList();

            return Ok(invoices);
        }


        [HttpGet("{id}")]
        public ActionResult<object> GetInvoice(int id)
        {
            var invoice = _context.Invoices
                .Include(i => i.Customer)
                .Include(i => i.Items)
                .FirstOrDefault(i => i.Id == id);

            if (invoice is null)
                return NotFound();

            var response = new
            {
                InvoiceId = invoice.Id,
                CreatedDate = invoice.CreatedDate,
                PaymentStatus = invoice.PaymentStatus,
                PaymentTerm = invoice.PaymentTerm,
                Description = invoice.Description,
                CustomerName = invoice.Customer.FullName,
                CustomerAddress = invoice.Customer.Address,
                CustomerEmail = invoice.Customer.Email,
                CustomerCity = invoice.Customer.City,
                CustomerCountry = invoice.Customer.Country,
                CustomerPostCode = invoice.Customer.PostCode,
                Items = invoice.Items.Select(item => new
                {
                    ItemId = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    Quantity = item.Quantity,
                    Price = item.Price,
                    Total = item.Total
                }).ToList(),
                TotalAmount = invoice.Items.Sum(item => item.Total)
            };

            return Ok(response);
        }

        [HttpPost]
        public ActionResult<object> AddInvoice([FromBody] DtoInvoiceCreateRequest invoiceRequest)
        {
            var invoice = new Invoice
            {
                CreatedDate = invoiceRequest.CreatedDate,
                PaymentStatus = invoiceRequest.PaymentStatus,
                PaymentTerm = (PaymentTerm)invoiceRequest.PaymentTerm,
                CustomerId = invoiceRequest.CustomerId,
                ClientId = 3,
                Description = invoiceRequest.Description
            };

            var items = _context.Items
                .Where(item => invoiceRequest.ItemIds.Contains(item.Id))
                .ToList();

            invoice.Items = items;

            invoice.TotalAmount = items.Sum(item => item.Price * item.Quantity);

            _context.Invoices.Add(invoice);
            _context.SaveChanges();

            var response = new
            {
                InvoiceId = invoice.Id,
                CreatedDate = invoice.CreatedDate,
                PaymentStatus = invoice.PaymentStatus,
                PaymentTerm = invoice.PaymentTerm,
                Description = invoice.Description,
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
                    Price = i.Price,
                    Total = i.Price * i.Quantity
                }).ToList(),
                TotalAmount = invoice.TotalAmount
            };

            return CreatedAtAction(nameof(GetInvoice), new { id = invoice.Id }, response);
        }

        [HttpPut]
        public ActionResult<Invoice> UpdateInvoice([FromBody] DtoInvoiceUpdateRequest invoiceRequest)
        {
            var invoice = _context.Invoices
                .Include(i => i.Items)
                .FirstOrDefault(i => i.Id == invoiceRequest.Id);

            if (invoice is null)
                return NotFound();

            invoice.CreatedDate = invoiceRequest.CreatedDate;
            invoice.PaymentStatus = invoiceRequest.PaymentStatus;
            invoice.PaymentTerm = (PaymentTerm)invoiceRequest.PaymentTerm;
            invoice.CustomerId = invoiceRequest.CustomerId;
            invoice.ClientId = 3;
            invoice.Description = invoiceRequest.Description;
            invoice.UpdatedDate = DateTime.Now;

            _context.SaveChanges();

            return Ok(invoice);
        }


        [HttpDelete("{id}")]
        public IActionResult DeleteInvoice(int id)
        {
            var invoice = _context.Invoices.Include(i => i.Items).FirstOrDefault(i => i.Id == id);

            if (invoice is null)
                return NotFound();

            _context.Invoices.Remove(invoice);
            _context.SaveChanges();

            return NoContent(); 
        }

    }
}
