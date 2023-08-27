namespace Damaris.DataService.Domain.v1.Models.Generic
{
    public class AppSettings
    {
        public ConnectionStrings ConnectionStrings { get; set; }
    }

    public class ConnectionStrings
    {
        public string OfficerMySqlGe2 { get; set; }
        public string WatchDogPostGreSql { get; set; }
    }
}
