using EShop.Application.Repositories.ProductRepository;
using EShop.Application.Repositories.UnitOfWork;
using EShop.Persistence.Contexts;
using EShop.Persistence.Repositories.ProductRepositoy;

namespace EShop.Persistence.Repositories.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
	private readonly EShopDbContext _context;

	public IProductReadRepository ReadProducts { get; private set; }
	public IProductWriteRepository WriteProducts { get; private set; }

	public UnitOfWork(EShopDbContext context)
	{
		_context = context;
		ReadProducts = new ProductReadRepository(_context);
		WriteProducts = new ProductWriteRepository(_context);
		// Initialize other repositories
	}

	public async Task<int> SaveChangesAsync()
	{
		return await _context.SaveChangesAsync();
	}
}
