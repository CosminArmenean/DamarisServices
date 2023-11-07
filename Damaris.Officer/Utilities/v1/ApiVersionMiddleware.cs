namespace Damaris.Officer.Utilities.v1
{
    public class ApiVersionMiddleware
    {
        private readonly RequestDelegate next;

        public ApiVersionMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            // Extract API version from request headers or query string
            string? apiVersion = context.Request.Headers["api-version"].ToString() ?? context.Request.Query["api-version"];

            // Set the API version in the context
            context.Items["ApiVersion"] = apiVersion;

            await next(context);
        }
    }
}
