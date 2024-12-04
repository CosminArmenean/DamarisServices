namespace Damaris.Officer.Configuration
{
    public class Globals : IGlobals
    {
        public string CLIENT_ID { get; set; } = "AngularInteractiveClient";
        public string CLIENT_URI { get; set; } = "localhost:4200";
        public string CLIENT_REDIRECT_URI { get; set; } = "http://localhost:4200";
        public string CLIENT_POST_LOGOUT_REDIRECT_URI { get; set; } = "http://localhost:4200";
        public string[] CLIENT_ALLOWED_CORS_ORIGINS { get; set; } = { "http://localhost:4200" };
        public string CERTIFICATE_PASSWORD { get; set; } // Password for the cer or pfx certificate included in the wwwRoot folder for IdentityServer

        public string API_RESOURCE_NAME { get; set; } = "innkt.resource";
        //public string API_RESOURCE_SECRET { get; set; }
        public string API_RESOURCE_SCOPE { get; set; } = "innkt.resource.scope";
        public string[] WEBAPI_REQUESTED_SCOPES { get; set; } = { "openid", "profile", "roles", "innkt.resource.scope" };

        // Connection string for SQL server
        public string CONNECTION_STRING { get; set; } = "Server=localhost;Database=officer;Uid=CosminArmenean;Pwd=@CAvp57rt26;";

        public string IDENTITYSERVER_HTTP_URI { get; set; } = "http://localhost:44383";
        public string IDENTITYSERVER_HTTPS_URI { get; set; } = "https://localhost:5001";
        public string WEBAPP_URI { get; set; } = "http://localhost:4200";
        public string WEBAPI_HTTP_URI { get; set; } = "http://localhost:5002";
        public string WEBAPI_HTTPS_URI { get; set; } = "https://localhost:5003";

        public string ADMIN_USERNAME { get; set; } = "admin";
        public string ADMIN_PASSWORD { get; set; }

        // These are Default Claims for the Admin User
        public string ADMIN_USER_FULL_NAME { get; set; } = "Cosmin Armenean";
        public string ADMIN_USER_GIVEN_NAME { get; set; } = "Cosmin";
        public string ADMIN_USER_FAMILY_NAME { get; set; } = "Armenean";
        public string ADMIN_USER_EMAIL { get; set; } = "cosmin.bibol@gmail.com";
        public string ADMIN_USER_WEBSITE { get; set; } = "https://www.innkt.com";
        public string ADMIN_ROLE { get; set; } = "admin"; // Roles are not required but included for convenience
    }
}
