using DataAccess;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CBTDWeb.Pages.Manufacturers
{
    public class UpsertModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        [BindProperty]
        public Manufacturer ? objManufacturer { get; set; }
        public UpsertModel(ApplicationDbContext db)
        {
            _db = db;
            objManufacturer = new Manufacturer();
        }

        public IActionResult OnGet(int?id)
        {
            if (id != 0)
            {
                objManufacturer = _db.Manufacturers.Find(id);
            }
            if (id == null)
            {
                return NotFound();
            }
            return Page();
        }
        public IActionResult OnPost()
        {
            if(objManufacturer.Id == 0)
            {
                _db.Manufacturers.Add(objManufacturer);
                TempData["success"] = "Manufacturer added Successfully";
            }
            else
            {
                _db.Manufacturers.Update(objManufacturer);
                TempData["success"] = "Manufacturer updated Successfully";
            }
            _db.SaveChanges();

            return RedirectToPage("./Index");
        }
    }
    
}
