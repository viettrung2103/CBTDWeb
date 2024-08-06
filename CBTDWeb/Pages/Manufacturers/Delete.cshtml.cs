using DataAccess;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CBTDWeb.Pages.Manufacturers
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        [BindProperty]
        public Manufacturer ? objManufacturer { get; set; }
        public DeleteModel(ApplicationDbContext db)
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
            if (!ModelState.IsValid)
            {
                return Page();
            }
            _db.Manufacturers.Remove(objManufacturer);  //Removes from memory
            TempData["success"] = "Manufacturer Deleted Successfully";
            _db.SaveChanges();   //saves to DB

            return RedirectToPage("./Index");
        }
    }
    
}
