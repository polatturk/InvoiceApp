using InvoiceApp.Data;
using InvoiceApp.Dto;
using InvoiceApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceApp.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public List<User> GetUsers()
        {
            List<User> user = _context.Users.ToList();
            return user;
        }

        [HttpGet("{id}")]
        public ActionResult<User> GetUser(int id)
        {
            var user = _context.Items.FirstOrDefault();

            if (user is null)
                return NotFound();

            return Ok(user);
        }

        [HttpPost]
        
        public ActionResult<User> AddUser([FromBody] DtoUserCreateRequest userrequest)
        {
            if (string.IsNullOrEmpty(userrequest.FullName))
            {
                return BadRequest("Geçersiz kullanici verisi.");
            }

            var User = new User()
            {
                FullName = userrequest.FullName,
                Address = userrequest.Address,
                PostCode = userrequest.PostCode,
                City = userrequest.City,
                Country = userrequest.Country,
                Email = userrequest.Email
            };

            _context.Users.Add(User);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetUser), new { id = userrequest.Id }, userrequest);
        }

        [HttpDelete("{id}")]
        public bool DeleteEducation(int id)
        {
            var user = _context.Users.Find(id);
            if (user is null)
                return false;
            _context.Users.Remove(user);
            _context.SaveChanges();
            return true;
        }
    }
}
