using Damaris.Domain.v1.Models.User;
using Damaris.Officer.Domain.v1;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Damaris.Officer.Data.v1
{
    public class OfficerDbContext : IdentityDbContext
    {
        //public virtual DbSet<IdentityUser> Users { get; set; }

        public OfficerDbContext(DbContextOptions<OfficerDbContext> options) : base(options) { }

        

    }
}
