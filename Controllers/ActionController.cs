using Microsoft.AspNetCore.Mvc;
using RateLimiter.Filters;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System;
using System.Threading.Tasks;

namespace csharp_client.Controllers
{
    public class ActionController : Controller
    {        
        [RateLimiterRPCFilter]
        public void ActionOne()
        {
            Debug.WriteLine("taking action one");                       
        }

        [RateLimiterRESTFilter]
        public void ActionTwo()
        {           
            Debug.WriteLine("taking action two");
        }

        [RateLimiterRPCFilter]
        public void ActionThree()
        {
            Debug.WriteLine("taking action three");
        }
    }
}
