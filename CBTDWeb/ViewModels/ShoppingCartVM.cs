using Infrastructure.Models;
using System.ComponentModel.DataAnnotations;

namespace CBTDWeb.ViewModels
{
    public class ShoppingCartVM
    {
        public Product? Product { get; set; }
        [Range(1, 1000, ErrorMessage = "Must be between 1 and 1000")]
        public int Count { get; set; }
        public IEnumerable<ShoppingCart>? cartItems { get; set; }
        // the below line is not inside the instruction
        public double CartTotal { get; set; }



    }

}
