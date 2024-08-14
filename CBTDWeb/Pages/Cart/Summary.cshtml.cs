
using CBTDWeb.ViewModels;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Utility;


namespace CBTDWeb.Pages.Cart
{
    public class SummaryModel : PageModel
    {
        private readonly UnitOfWork _unitOfWork;

        [BindProperty]
        public ShoppingCartVM? ShoppingCartVM { get; set; }
        public SummaryModel(UnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public IActionResult OnGet()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCartVM = new ShoppingCartVM()
            {
                cartItems = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value,
                includes: "Product"),
                OrderHeader = new()
            };
            ShoppingCartVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.Get(
                u => u.Id == claim.Value);
            ShoppingCartVM.OrderHeader.Name = ShoppingCartVM.OrderHeader.ApplicationUser.FullName;
            ShoppingCartVM.OrderHeader.PhoneNumber = ShoppingCartVM.OrderHeader.ApplicationUser.PhoneNumber;
            ShoppingCartVM.OrderHeader.StreetAddress = ShoppingCartVM.OrderHeader.ApplicationUser.StreetAddress;
            ShoppingCartVM.OrderHeader.City = ShoppingCartVM.OrderHeader.ApplicationUser.City;
            ShoppingCartVM.OrderHeader.State = ShoppingCartVM.OrderHeader.ApplicationUser.State;
            ShoppingCartVM.OrderHeader.PostalCode = ShoppingCartVM.OrderHeader.ApplicationUser.PostalCode;

            foreach (var cart in ShoppingCartVM.cartItems)
            {
                cart.CartPrice = ShoppingCartVM.GetPriceBasedOnQuantity(cart.Count, cart.Product.UnitPrice,
                    cart.Product.HalfDozenPrice, cart.Product.DozenPrice);
                ShoppingCartVM.OrderHeader.OrderTotal += cart.CartPrice * cart.Count;

            }
            return Page();
        }
		public IActionResult OnPost()
		{
			var claimsIdentity = User.Identity as ClaimsIdentity;
			var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

			//The Order is actually the shopping cart items, so we might as well use it

			ShoppingCartVM.cartItems = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value,
					includes: "Product");

			ShoppingCartVM.OrderHeader.OrderDate = System.DateTime.Now;
			ShoppingCartVM.OrderHeader.ApplicationUserId = claim.Value;

			foreach (var cart in ShoppingCartVM.cartItems)
			{
				cart.CartPrice = ShoppingCartVM.GetPriceBasedOnQuantity(cart.Count, cart.Product.UnitPrice,
				cart.Product.HalfDozenPrice, cart.Product.DozenPrice);
				ShoppingCartVM.OrderHeader.OrderTotal += cart.CartPrice * cart.Count;
			}
			ApplicationUser applicationUser = _unitOfWork.ApplicationUser.Get(u => u.Id == claim.Value);

			ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
			ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusInProcess;

			_unitOfWork.OrderHeader.Add(ShoppingCartVM.OrderHeader);
			_unitOfWork.Commit();

			//Once the Order Header is created, we can start cycling through our Order Details

			foreach (var cart in ShoppingCartVM.cartItems)
			{
				OrderDetails orderDetail = new()
				{
					ProductId = cart.ProductId,
					OrderId = ShoppingCartVM.OrderHeader.Id,
					Price = cart.CartPrice,
					Count = cart.Count
				};
				_unitOfWork.OrderDetails.Add(orderDetail);
				_unitOfWork.Commit();
			}

			//don't forget to clear the physical shopping cart entries

			_unitOfWork.ShoppingCart.Delete(ShoppingCartVM.cartItems);
			_unitOfWork.Commit();
			return RedirectToPage("OrderConfirmation", new { orderId = ShoppingCartVM.OrderHeader.Id });
		}
	}

}


