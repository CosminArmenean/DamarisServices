using Microsoft.AspNetCore.Mvc.Filters;

namespace Damaris.Frontier.Configurations.Filters
{
    public class IdentityFilter : IActionFilter
    {

        public void OnActionExecuting(ActionExecutingContext context)
        {
            Console.WriteLine($"  This global  Filter executed on OnActionExecuting");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            Console.WriteLine($"  This global  Filter executed on OnActionExecuted");
        }

        
    }
}
