using CBTDWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Org.BouncyCastle.Tls;
using System.Security.Claims;

namespace CBTDWeb.Pages.Cart
{
    public class IndexModel : PageModel
    {
        private readonly UnitOfWork _unitOfWork;
        // by inject this Model to this page, this page can also access other models inside the ShoppingCartVM

        public ShoppingCartVM? ShoppingCartVM { get; set; }

        // normal is to initialize the Model >> shoppingCartVM = new ShoppingCartVm
        // However, since this is the View Model, we will have many models inside 
        // having a initializing function to handle these underlying model is better
        public IndexModel(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            InitializeShoppingCartVM();
        }

        private void InitializeShoppingCartVM()
        {
            ShoppingCartVM = new ShoppingCartVM
            {
                cartItems = []
                //Count = 0,
                //CartTotal = 0
            };
        }


        public IActionResult OnGet()
        {
            // check if the user still logged in
            var claimsIdentity = User.Identity as ClaimsIdentity;

            var claim = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCartVM = new ShoppingCartVM()
            {
                // the returned querry is stored inside variable u
                // which is filtered by ApplicationUserId == claim.Value (which is Id of the first return value)
                // afterwards, it is sorted using the ProductId,
                // the last one is "Include", which is to join table "Product" as well
                cartItems = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value, u => u.ProductId, "Product")
            };
            foreach (var item in ShoppingCartVM.cartItems)
            {
                item.CartPrice = ShoppingCartVM.GetPriceBasedOnQuantity(item.Count, item.Product.UnitPrice, item.Product.HalfDozenPrice, item.Product.DozenPrice);
                ShoppingCartVM.CartTotal += (item.CartPrice * item.Count);

            }



            return Page();
        }



        public IActionResult OnPostMinus(int cartId)
        {
            var cart = _unitOfWork.ShoppingCart.Get(c => c.Id == cartId);
            if (cart.Count == 1) // when Count =1 > -1 = 0 >> Delete
            {
                _unitOfWork.ShoppingCart.Delete(cart);
            }
            else
            {
                cart.Count -= 1;
                _unitOfWork.ShoppingCart.Update(cart);
            }
            _unitOfWork.Commit();
            var cnt = _unitOfWork.ShoppingCart.GetAll(
                user => user.ApplicationUserId == cart.ApplicationUserId
                )
                .Count();
            return RedirectToPage();
        }

        public IActionResult OnPostPlus(int cartId)
        {
            var cart = _unitOfWork.ShoppingCart.Get(c => c.Id == cartId);
            cart.Count += 1;
            _unitOfWork.ShoppingCart.Update(cart);
            _unitOfWork.Commit();
            return RedirectToPage();
        }
        public IActionResult OnPostRemove(int cartId)
        {
            var cart = _unitOfWork.ShoppingCart.Get(c => c.Id == cartId);
            _unitOfWork.ShoppingCart.Delete(cart);
            _unitOfWork.Commit();
            var cnt = _unitOfWork.ShoppingCart.GetAll(
                u => u.ApplicationUserId == cart.ApplicationUserId)
                .Count();
            return RedirectToPage();
        }

    }
}
