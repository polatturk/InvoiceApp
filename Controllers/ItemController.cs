using InvoiceApp.Data;
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
            var item = _context.Items.FirstOrDefault(e => e.InvoiceId == id);

            if (item is null)
                return NotFound();

            return Ok(item);
        }

        [HttpPost]
        public ActionResult<Item> AddItem([FromBody] Item item)
        {
            if (string.IsNullOrEmpty(item.Name) || item.Quantity <= 0 || item.Price <= 0)
            {
                return BadRequest("Geçersiz item verisi.");
            }

            _context.Items.Add(item);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetItem), new { id = item.Id }, item);
        }

        [HttpDelete("{id}")]
        public bool DeleteEducation(int id)
        {
            var education = _context.Items.Find(id);
            if (education is null)
                return false;
            _context.Items.Remove(education);
            _context.SaveChanges();
            return true;
        }
    }
}
