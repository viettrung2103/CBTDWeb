using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CBTDWeb.Pages.Products
{
    public class IndexModel : PageModel
    {
        //declare service and enumerate list (get one item at a time
        private readonly UnitOfWork _unitOfWork;
        public IEnumerable<Product> objProductList;

        //create a depency injection
        public IndexModel(UnitOfWork unitOfWork)
        {
            // do the depency injection
            _unitOfWork = unitOfWork;
            // to make sure we dont have a null value
            objProductList = new List<Product>();
        }

        public IActionResult OnGet()
        {
            objProductList = _unitOfWork.Product.GetAll();
            return Page();
        }

    }
}
