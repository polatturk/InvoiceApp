using Microsoft.AspNetCore.Mvc;

namespace InvoiceApp.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
