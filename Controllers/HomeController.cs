using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RateLimiter.Services;
using RateLimiter.Interfaces;
using Ratelimiter;


namespace csharp_client.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRateLimiterClient rateLimiterService;

        public HomeController(IRateLimiterClient rlService)
        {
            rateLimiterService = rlService;
        }
        public IActionResult Index()
        {
                       
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";            
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
        [Route("Home/Action/{ActionName}")]
        public JsonResult TakeAction(string ActionName)
        {
            var request = new Request();
            request.Key = ActionName;
            request.Expire = "60";            
            return Json(rateLimiterService.checkRequest(request));
        }
    }
}
