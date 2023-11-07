namespace Damaris.Officer.Utilities.v1.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class CustomApiVersionAttribute : Attribute
    {
        public string ApiVersion { get; set; }

        public bool Matches(HttpContext context)
        {
            string contextVersion = (string)context.Items["ApiVersion"];
            return ApiVersion.Equals(contextVersion);
        }
    }
}
