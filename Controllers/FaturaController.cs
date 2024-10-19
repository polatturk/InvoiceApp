using Microsoft.AspNetCore.Mvc;

namespace InvoiceApp.Controllers
{
    public class FaturaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
