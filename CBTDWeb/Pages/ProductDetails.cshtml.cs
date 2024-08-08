using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CBTDWeb.Pages
{
    public class ProductDetailsModel : PageModel
    {
        private readonly UnitOfWork _unitOfWork; // inject

        public Product objProduct; //declare product

        [BindProperty]
        public int txtCount { get; set; } //??

        public ProductDetailsModel(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            objProduct = new Product();
        }

        // In the HTML page the asp-route-productId is why we get the int and the name
        public IActionResult OnGet(int? id)
        {
            //FOR OUTER JOIN , CATEGORY.MANUFACTUER, STUDENT = category left join manufacturer and join student
            objProduct = _unitOfWork.Product.Get(p => p.Id == id, includes: "Category,Manufacturer");
            // the p here stored the result of the sql querry
            // SELECT * FROM PRODUCTS P
            // JOIN CATEGORIES C ON C.PRODUCTID = P.PRODUCTID
            // JOIN MANUFACTURERS M ON M.PRODUCTID = P.PRODUCTID
            // WHERE P.ID = @ID
            return Page();
        }

    }
}
