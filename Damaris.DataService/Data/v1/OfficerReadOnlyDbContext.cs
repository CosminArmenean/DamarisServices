using Damaris.Domain.v1.Models.User;
using Microsoft.EntityFrameworkCore;

namespace Damaris.DataService.Data.v1
{
    public class OfficerReadOnlyDbContext : DbContext
    {
        public virtual DbSet<User> Users { get; set; }

        public OfficerReadOnlyDbContext(DbContextOptions<OfficerReadOnlyDbContext> options) : base(options) { }
    }
}
