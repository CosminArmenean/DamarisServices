using Damaris.Domain.v1.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Damaris.Officer.Data.v1
{
    public class OfficerDbContext : DbContext
    {
        public virtual DbSet<IdentityUser> Users { get; set; }

        public OfficerDbContext(DbContextOptions<OfficerDbContext> options) : base(options) { }

        

    }
}
