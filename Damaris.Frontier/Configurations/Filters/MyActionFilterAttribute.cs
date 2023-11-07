using Microsoft.AspNetCore.Mvc.Filters;

namespace Damaris.Frontier.Configurations.Filters
{
    public class MyActionFilterAttribute : Attribute, IActionFilter
    {
        private readonly string _callerName;

        public MyActionFilterAttribute(string callerName)
        {
            _callerName = callerName;
        }
        public void OnActionExecuting(ActionExecutingContext context)
        {
            Console.WriteLine($" {_callerName} - This Filter executed on OnActionExecuting");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            Console.WriteLine($" {_callerName} - This Filter executed on OnActionExecuted");
        }

        
    }
}
