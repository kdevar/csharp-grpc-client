using Microsoft.AspNetCore.Mvc;
using RateLimiter.Interfaces;


namespace csharp_client.Controllers
{
    public class HomeController : Controller
    {        
        public IActionResult Index()
        {                       
            return View();
        }
        public IActionResult Error()
        {
            return View();
        }
       
    }
}
