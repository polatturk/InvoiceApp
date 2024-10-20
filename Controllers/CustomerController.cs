using InvoiceApp.Data;
using InvoiceApp.Dto;
using InvoiceApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceApp.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CustomerController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CustomerController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public List<Customer> GetCustomers()
        {
            List<Customer> customer = _context.Customers.ToList();
            return customer;
        }

        [HttpGet("{id}")]
        public ActionResult<Customer> GetCustomer(int id)
        {
            var customer = _context.Customers.FirstOrDefault();

            if (customer is null)
                return NotFound();

            return Ok(customer);
        }

        [HttpPost]
        public ActionResult<Customer> AddCustomer([FromBody] DtoCustomerCreateRequest customeRequest)
        {
            var customer = new Customer
            {
                FullName = customeRequest.FullName,
                Email = customeRequest.Email,
                Address = customeRequest.Address,
                City = customeRequest.City,
                Country = customeRequest.Country,
                PostCode = customeRequest.PostCode,
            };

            _context.Customers.Add(customer);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetCustomer), new { id = customer.Id }, customer);
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