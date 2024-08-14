using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{

	public interface IShoppingCartRepository<T> : IGenericRepository<ShoppingCart>

	{
		int IncrementCount(ShoppingCart shoppingCart, int count);
		int DecrementCount(ShoppingCart shoppingCart, int count);
	}
}
