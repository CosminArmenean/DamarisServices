using Damaris.Domain.v1.Dtos.UserDtos;
using Microsoft.EntityFrameworkCore;

namespace Damaris.DataService.Data.v1
{
    public class OfficerDbContext : DbContext
    {
        public virtual DbSet<UserDto> Users { get; set; }

        public OfficerDbContext(DbContextOptions<OfficerDbContext> options) : base(options) { }
        

    }
}
