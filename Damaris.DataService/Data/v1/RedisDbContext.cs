using Damaris.Domain.v1.Models.User;
using Microsoft.EntityFrameworkCore;

namespace Damaris.DataService.Data.v1
{
    public class RedisDbContext : DbContext
    {
        

        public RedisDbContext(DbContextOptions options) : base(options)
        {
            
        }
        

    }
}
