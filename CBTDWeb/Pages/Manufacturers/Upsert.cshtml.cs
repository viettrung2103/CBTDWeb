using DataAccess;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CBTDWeb.Pages.Manufacturers
{
    public class UpsertModel : PageModel
    {
        private readonly UnitOfWork _unitOfWork;
        [BindProperty]
        public Manufacturer ? objManufacturer { get; set; }
        public UpsertModel(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            objManufacturer = new Manufacturer();
        }

        public IActionResult OnGet(int?id)
        {
            if (id != 0)
            {
                objManufacturer = _unitOfWork.Manufacturer.GetById(id);
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
                _unitOfWork.Manufacturer.Add(objManufacturer);
                TempData["success"] = "Manufacturer added Successfully";
            }
            else
            {
                _unitOfWork.Manufacturer.Update(objManufacturer);
                TempData["success"] = "Manufacturer updated Successfully";
            }
            _unitOfWork.Commit();

            return RedirectToPage("./Index");
        }
    }
    
}
