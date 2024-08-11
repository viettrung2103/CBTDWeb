using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace CBTDWeb.Pages
{
    public class ProductDetailsModel : PageModel
    {
        private readonly UnitOfWork _unitOfWork; // inject

        public Product objProduct; //declare product

        [BindProperty]
        // why need BindProperty, 
        public int txtCount { get; set; } //??
        public ShoppingCart objCart = new();


        public ProductDetailsModel(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            objProduct = new Product();
        }

        // In the HTML page the asp-route-productId is why we get the int and the name
        public IActionResult OnGet(int? id)
        {
            //check to see if user logged on
            var claimsIdentity = User.Identity as ClaimsIdentity; // get the user object that is returned when logged >> might be a list of users
            var claim = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier); //grab the logged in user, normally at idx 0
            TempData["UserLoggedIn"] = claim;

            //FOR OUTER JOIN , CATEGORY.MANUFACTUER, STUDENT = category left join manufacturer and join student
            objProduct = _unitOfWork.Product.Get(p => p.Id == id, includes: "Category,Manufacturer");
            // the p here stored the result of the sql querry
            // SELECT * FROM PRODUCTS P
            // JOIN CATEGORIES C ON C.PRODUCTID = P.PRODUCTID
            // JOIN MANUFACTURERS M ON M.PRODUCTID = P.PRODUCTID
            // WHERE P.ID = @ID
            return Page();
        }

        //way without BindProperty
        //public IActionResult OnPost(Product objProduct, int txtCount)
        
        //way with BindProperty
        public IActionResult OnPost(Product objProduct)
        {
            // grab the user identity >> to make sure that when the form is submitted, the user is still logged in
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var userId = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var existingCart = _unitOfWork.ShoppingCart.Get(
                u => u.ApplicationUserId == userId && u.ProductId == objProduct.Id);
            // the reason for u.ProductId == objProdct.Id >> to add to the current amount of that product

            if (existingCart == null)
            {
                var newCart = new ShoppingCart
                {
                    ApplicationUserId = userId,
                    ProductId = objProduct.Id,
                    Count = txtCount
                };

                _unitOfWork.ShoppingCart.Add(newCart);
            }
            else //shoping cart entry with the request product is existed in db
            {
                _unitOfWork.ShoppingCart.IncrementCount(existingCart, txtCount);
                _unitOfWork.ShoppingCart.Update(existingCart);
            }
            //physical saving to database
            _unitOfWork.Commit();
            return RedirectToPage("Index");
        }


    }
}
