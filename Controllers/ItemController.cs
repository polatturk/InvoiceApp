using InvoiceApp.Data;
using InvoiceApp.Dto;
using InvoiceApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InvoiceApp.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ItemController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ItemController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public List<Item> GetItems()
        {
            List<Item> items = _context.Items.ToList();
            return items;
        }

        [HttpGet("{id}")]
        public ActionResult<Item> GetItem(int id)
        {
            var item = _context.Items.FirstOrDefault();

            if (item is null)
                return NotFound();

            return Ok(item);
        }

        [HttpPost]
        public ActionResult<Item> AddItem([FromBody] DtoItemCreateRequest itemRequest)
        {
            if (string.IsNullOrEmpty(itemRequest.Name))
            {
                return BadRequest("Geçersiz kullanıcı verisi.");
            }

            var invoice = _context.Invoices.Find(itemRequest.InvoiceId);
            if (invoice == null)
            {
                return NotFound("Geçersiz fatura ID'si.");
            }

            var item = new Item
            {
                Name = itemRequest.Name,
                Description = itemRequest.Description,
                Quantity = itemRequest.Quantity,
                Price = itemRequest.Price,
                InvoiceId = itemRequest.InvoiceId,
                Total = itemRequest.Quantity * itemRequest.Price
            };

            _context.Items.Add(item);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetItem), new { id = item.Id }, item);
        }

        [HttpDelete("{id}")]
        public bool DeleteItem(int id)
        {
            var item = _context.Items.Find(id);
            if (item is null)
                return false;
            _context.Items.Remove(item);
            _context.SaveChanges();
            return true;
        }
    }
}