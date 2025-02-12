﻿using Infrastructure.Models;

namespace Infrastructure.Interfaces
{
    public interface IUnitOfWork
    {
        //ADD Models/Tables here as you create them so UnitOfWork will have access
        public IGenericRepository<Category> Category { get; }
        public IGenericRepository<Manufacturer> Manufacturer { get; }
      
        public IGenericRepository<Product> Product { get; }
        public IGenericRepository<ApplicationUser> ApplicationUser { get; }

        public IShoppingCartRepository<ShoppingCart> ShoppingCart { get; }

        public IGenericRepository<OrderDetails> OrderDetails { get; }
        public IOrderHeaderRepository<OrderHeader> OrderHeader { get; }


        //save changes to the data source

        int Commit();

        Task<int> CommitAsync();

    }
}
