using Microsoft.AspNetCore.Mvc.Filters;

namespace Damaris.Officer.Configuration.Filters
{
    
    public class IdentityUserFilterAttribute : Attribute, IAsyncActionFilter, IOrderedFilter
    {
        private readonly string _callerName;
        public int Order { get; set; }
        public IdentityUserFilterAttribute(string callerName, int order)
        {
            _callerName = callerName;
            Order = order;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            Console.WriteLine($" {_callerName} - Async Filter durin execution");
            await next();
            Console.WriteLine($" {_callerName} - Async Filter after execution");
        }


    }
}
