using InvoiceApp.Data;
using InvoiceApp.Dto;
using InvoiceApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace InvoiceApp.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ClientController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ClientController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<List<DtoClientCreateRequest>> GetClients()
        {
            var clients = _context.Client
                .Include(c => c.Customers)  
                .Include(c => c.Invoices) 
                .Include(c => c.Items)        
                .ToList();

            var clientDtos = clients.Select(c => new DtoClientCreateRequest
            {
                Id = c.Id,
                Name = c.Name,
                Adress = c.Adress,
                City = c.City,
                PostCode = c.PostCode,
                Country = c.Country,
                Customers = c.Customers.Select(customer => new DtoCustomerCreateRequest
                {
                    FullName = customer.FullName,
                    Email = customer.Email,
                    Address = customer.Address,
                    City = customer.City,
                    PostCode = customer.PostCode,
                    Country = customer.Country
                }).ToList(),
                Invoices = c.Invoices.Select(invoice => new DtoInvoiceCreateRequest
                {
                    Id = invoice.Id,
                    CreatedDate = invoice.CreatedDate,
                    PaymentStatus = invoice.PaymentStatus,
                    PaymentTerm = invoice.PaymentTerm
                }).ToList(),
                Items = c.Items.Select(item => new DtoItemCreateRequest
                {
                    Name = item.Name,
                    Price = item.Price,
                    Quantity = item.Quantity
                }).ToList()
            }).ToList();

            return Ok(clientDtos);
        }


        [HttpGet("{id}")]
        public ActionResult<Client> GetClient(int id)
        {
            var client = _context.Client.FirstOrDefault(c => c.Id == id);
            if (client == null)
            {
                return NotFound();
            }
            return Ok(client);
        }
            //[HttpPost]
            //public ActionResult<object> AddClient([FromBody] DtoClientCreateRequest clientrequest)
            //{
            //    var client = new Client
            //    {
            //        Name = clientrequest.Name,
            //        Adress = clientrequest.Adress,
            //        City = clientrequest.City,
            //        Country = clientrequest.Country,
            //        PostCode = clientrequest.PostCode,
            //    };


            //    _context.Client.Add(client);
            //    _context.SaveChanges();

            //    var response = new
            //    {
            //        Name = clientrequest.Name,
            //        Adress = clientrequest.Adress,
            //        City = clientrequest.City,
            //        Country = clientrequest.Country,
            //        PostCode = clientrequest.PostCode,
            //    };

            //    return CreatedAtAction(nameof(GetClient), new { id = client.Id }, response);
            //}


        [HttpPut("{id}")]
        public ActionResult<Client> UpdateClient(int id, [FromBody] Client updatedClient)
        {
            var client = _context.Client.FirstOrDefault(c => c.Id == id);
            if (client == null)
            {
                return NotFound();
            }

            client.Name = updatedClient.Name;
            client.Adress = updatedClient.Adress;
            client.City = updatedClient.City;
            client.PostCode = updatedClient.PostCode;
            client.Country = updatedClient.Country;

            _context.SaveChanges();
            return Ok(client);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteClient(int id)
        {
            var client = _context.Client.Find(id);
            if (client == null)
            {
                return NotFound();
            }

            _context.Client.Remove(client);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
