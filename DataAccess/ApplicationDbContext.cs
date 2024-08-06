using Infrastructure.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class ApplicationDbContext : IdentityDbContext
    {
        // constructor to initiate the database object and map to the actual database
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
    }
            public DbSet<Category> Categories { get; set; }  //the physical DB table will be named Categories, while "Category" is the name that will be used in code
            public DbSet<Manufacturer> Manufacturers { get; set; }
    }
}
