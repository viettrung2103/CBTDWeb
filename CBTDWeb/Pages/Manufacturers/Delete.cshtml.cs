using DataAccess;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CBTDWeb.Pages.Manufacturers
{
    public class DeleteModel : PageModel
    {
        private readonly UnitOfWork _UniOfWork;
        [BindProperty]
        public Manufacturer ? objManufacturer { get; set; }
        public DeleteModel(UnitOfWork unitOfWork)
        {
            _UniOfWork = unitOfWork;
            objManufacturer = new Manufacturer();
        }

        public IActionResult OnGet(int?id)
        {
            if (id != 0)
            {
                objManufacturer = _UniOfWork.Manufacturer.GetById(id);
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
            _UniOfWork.Manufacturer.Delete(objManufacturer);  //Removes from memory
            TempData["success"] = "Manufacturer Deleted Successfully";
            _UniOfWork.Commit();   //saves to DB

            return RedirectToPage("./Index");
        }
    }
    
}
