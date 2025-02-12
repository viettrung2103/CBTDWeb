﻿using DataAccess;
using Infrastructure.Interfaces;
using Infrastructure.Models;


// this layer is a mapping from the C# code to the physical database
//
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext;  //dependency injection of Data Source

    public UnitOfWork(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    private IGenericRepository<Category> _Category;
    private IGenericRepository<Manufacturer> _Manufacturer;
    private IGenericRepository<Product> _Product;
    private IGenericRepository<ApplicationUser> _ApplicationUser;
    //private IGenericRepository<ShoppingCart> _ShoppingCart;
    private IShoppingCartRepository<ShoppingCart> _ShoppingCart;
    private IGenericRepository<OrderDetails> _OrderDetails;

    private IOrderHeaderRepository<OrderHeader> _OrderHeader;

    public IGenericRepository<Category> Category
    {
        get
        {

            if (_Category == null)
            {
                _Category = new GenericRepository<Category>(_dbContext);
            }

            return _Category;
        }
    }

    public IGenericRepository<Manufacturer> Manufacturer
    {
        get
        {

            if (_Manufacturer == null)
            {
                _Manufacturer = new GenericRepository<Manufacturer>(_dbContext);
            }

            return _Manufacturer;
        }
    }

    public IGenericRepository<Product> Product
    {
        get
        {

            if (_Product == null)
            {
                _Product = new GenericRepository<Product>(_dbContext);
            }

            return _Product;
        }
    }
    //Constructor
    public IGenericRepository<ApplicationUser> ApplicationUser
    {
        get
        {

            if (_ApplicationUser == null)
            {
                _ApplicationUser = new GenericRepository<ApplicationUser>(_dbContext);
            }

            return _ApplicationUser;
        }
    }



	public IShoppingCartRepository<ShoppingCart> ShoppingCart
	{
		get
		{
			if (_ShoppingCart== null)
			{
				_ShoppingCart= new ShoppingCartRepository(_dbContext);
			}
			return _ShoppingCart;
		}
	}

	public IGenericRepository<OrderDetails> OrderDetails
    {
        get
        {

            if (_OrderDetails == null)
            {
                _OrderDetails = new GenericRepository<OrderDetails>(_dbContext);
            }

            return _OrderDetails;
        }
    }

    public IOrderHeaderRepository<OrderHeader> OrderHeader
    {
        get
        {
            if (_OrderHeader == null)
            {
                _OrderHeader = new OrderHeaderRepository(_dbContext);
            }
            return _OrderHeader;
        }
    }

  

    //ADD ADDITIONAL METHODS FOR EACH MODEL HERE

    public int Commit()
    {
        return _dbContext.SaveChanges();
    }

    public async Task<int> CommitAsync()
    {
        return await _dbContext.SaveChangesAsync();
    }

    //not only inherate, can create a function
    //additional method added for garbage disposal

    public void Dispose()
    {
        _dbContext.Dispose();
    }
}
