using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RateLimiter.Interfaces;
using Ratelimiter;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System;
using System.Threading.Tasks;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace RateLimiter.Filters
{
    public class RateLimiterRPCFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //probably not the right way to do this - must be some way to have the container inject
            IRateLimiterService service = context.HttpContext.RequestServices.GetService<IRateLimiterService>();
            Request request = RateLimiterHelper.CreateRequestFromContext(context);
            State rateLimitResponse = service.checkRequest(request);
            RateLimiterHelper.HandleResponse(rateLimitResponse, context);            
        }
    }

    public class RateLimiterRESTFilterAttribute: ActionFilterAttribute
    {        
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            Uri uri = new Uri("http://localhost:3001/check-request");
            Request request = RateLimiterHelper.CreateRequestFromContext(context);
            StringContent content = new StringContent(JsonConvert.SerializeObject(request, new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                }
            }), Encoding.UTF8, "application/json");                        
            HttpClient client = new HttpClient();
            HttpResponseMessage httpResponse = await  client.PostAsync(uri, content);
            String httpContent = await httpResponse.Content.ReadAsStringAsync();
            State rateLimitResponse = JsonConvert.DeserializeObject<State>(httpContent);
            RateLimiterHelper.HandleResponse(rateLimitResponse, context);
            await next();
        }
    }

    public class RateLimiterHelper
    {
        public static void HandleResponse(State rateLimitResponse, ActionExecutingContext context)
        {
            context.HttpContext.Response.Headers.Add("value", rateLimitResponse.NewValue.ToString());
            Debug.WriteLine("CURRENT VALUE = " + rateLimitResponse.NewValue.ToString());
            if (!rateLimitResponse.Available)
            {
                Debug.WriteLine("LIMIT REACHED");                
                context.HttpContext.Response.StatusCode = 429;
                context.Result = new ContentResult
                {
                    Content = "Too Many Requests"
                };                
            }
            else
            {
                Debug.WriteLine("there is still room");
            }
        }

        public static Request CreateRequestFromContext(ActionExecutingContext context)
        {
            Request request = new Request();
            request.Key = context.HttpContext.Request.Path;
            request.Expire = "60";
            return request;
        }
    }
}