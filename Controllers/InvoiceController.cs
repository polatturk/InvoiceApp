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

            foreach (var item in items)
            {
                var index = invoiceRequest.ItemIds.IndexOf(item.Id);
                var quantityToReduce = invoiceRequest.Quantities[index];

                if (item.Quantity >= quantityToReduce)
                {
                    item.Quantity -= quantityToReduce;

                    invoice.Items.Add(new Item
                    {
                        Name = item.Name,
                        Description = item.Description,
                        Quantity = quantityToReduce,
                        Price = item.Price,
                        Total = item.Price * quantityToReduce 
                    });
                }
                else
                {
                    return BadRequest($"Yetersiz miktar: {item.Name} için mevcut miktar: {item.Quantity}, istenen miktar: {quantityToReduce}.");
                }
            }

            invoice.TotalAmount = invoice.Items.Sum(i => i.Total);

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
                Items = invoice.Items.Select(i => new
                {
                    ItemId = i.Id,
                    Name = i.Name,
                    Description = i.Description,
                    Quantity = i.Quantity,
                    Price = i.Price,
                    Total = i.Total
                }).ToList(),
                TotalAmount = invoice.TotalAmount
            };

            return CreatedAtAction(nameof(GetInvoice), new { id = invoice.Id }, response);
        }

        [HttpPut]
        public ActionResult<object> UpdateInvoice([FromBody] DtoInvoiceUpdateRequest invoiceRequest)
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
            invoice.Description = invoiceRequest.Description;
            invoice.UpdatedDate = DateTime.Now;

            for (int i = 0; i < invoice.Items.Count; i++)
            {
                var item = invoice.Items[i];
                var newQuantity = invoiceRequest.Quantities[i];
                var dbItem = _context.Items.FirstOrDefault(x => x.Id == item.Id);

                if (dbItem != null)
                {
                    int quantityDifference = newQuantity - item.Quantity;

                    if (quantityDifference > 0 && dbItem.Quantity >= quantityDifference)
                    {
                        dbItem.Quantity -= quantityDifference; 
                    }
                    else if (quantityDifference < 0)
                    {
                        dbItem.Quantity += Math.Abs(quantityDifference);
                    }
                    else if (dbItem.Quantity < quantityDifference)
                    {
                        return BadRequest($"Yetersiz stok: {item.Name} için mevcut stok: {dbItem.Quantity}, istenen miktar: {newQuantity}.");
                    }

                    item.Quantity = newQuantity;
                    item.Total = item.Quantity * (double)item.Price;
                }
            }

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
