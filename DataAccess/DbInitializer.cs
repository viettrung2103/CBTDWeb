using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;


namespace DataAccess.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _db;

        public DbInitializer(ApplicationDbContext db)
        {
            _db = db;
        }


        public void Initialize()
        {
            _db.Database.EnsureCreated();

            //migrations if they are not applied
            try
            {
                if (_db.Database.GetPendingMigrations().Any())
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception)
            {

            }

            if (_db.Categories.Any())
            {
                return; //DB has been seeded, or the Categories tables is already existed
            }

            var Categories = new List<Category>
            {

            new Category { Name = "Non-Alcoholic Beverages", DisplayOrder = 1 },
            new Category { Name = "Wine", DisplayOrder = 2 },
            new Category { Name = "Snacks", DisplayOrder = 3 }
            };

            foreach (var c in Categories)
            {
                _db.Categories.Add(c);
            }
            _db.SaveChanges();
        }
    }
}
