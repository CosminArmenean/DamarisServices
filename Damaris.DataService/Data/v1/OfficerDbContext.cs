using Damaris.Domain.v1.Dtos.UserDtos;
using Damaris.Domain.v1.Models.User;
using Microsoft.EntityFrameworkCore;

namespace Damaris.DataService.Data.v1
{
    public class OfficerDbContext : DbContext
    {
        public virtual DbSet<User> Users { get; set; }

        public OfficerDbContext(DbContextOptions<OfficerDbContext> options) : base(options) { }
        

    }
}
