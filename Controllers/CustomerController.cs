﻿using InvoiceApp.Data;
using InvoiceApp.Dto;
using InvoiceApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public IActionResult GetCustomers()
        {
            var customers = _context.Customers
                .Include(c => c.Invoices)
                    .ThenInclude(i => i.Items) 
                .Select(c => new
                {
                    FullName = c.FullName,
                    Email = c.Email,
                    Address = c.Address,
                    City = c.City,
                    Country = c.Country,
                    PostCode = c.PostCode,
                    ClientId = c.ClientId,
                    Invoices = c.Invoices.Select(i => new
                    {
                        i.Id,
                        i.CreatedDate,
                        i.Description,
                        i.PaymentStatus,
                        i.PaymentTerm,
                        TotalAmount = i.Items.Sum(item => item.Total), 
                        Items = i.Items.Select(item => new
                        {
                            item.Id,
                            item.Name,
                            item.Description,
                            item.Quantity,
                            item.Price,
                            item.Total
                        }).ToList()
                    }).ToList()
                })
                .ToList();

            return Ok(customers);
        }


        [HttpGet("{id}")]
        public ActionResult<Customer> GetCustomer(int id)
        {
            var customer = _context.Customers.FirstOrDefault(c => c.Id == id);

            if (customer is null)
                return NotFound();

            return Ok(customer);
        }

        [HttpPost]
        public ActionResult<Customer> AddCustomer([FromBody] DtoCustomerCreateRequest customerRequest)
        {
            var defaultClient = _context.Client.FirstOrDefault();

            if (defaultClient == null)
            {
                return BadRequest("Varsayılan bir Client bulunamadı.");
            }

            var customer = new Customer
            {
                FullName = customerRequest.FullName,
                Email = customerRequest.Email,
                Address = customerRequest.Address,
                City = customerRequest.City,
                Country = customerRequest.Country,
                PostCode = customerRequest.PostCode,
                ClientId = defaultClient.Id  
            };

            _context.Customers.Add(customer);
            _context.SaveChanges();

            return Ok("Musteri eklendi.");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCustomer(int id)
        {
            var customer = _context.Customers.Find(id);
            if (customer is null)
            {
                return NotFound("Silinecek müşteri bulunamadı.");
            }

            _context.Customers.Remove(customer);
            _context.SaveChanges();

            return Ok("Silme işlemi başarılı.");
        }

    }
}