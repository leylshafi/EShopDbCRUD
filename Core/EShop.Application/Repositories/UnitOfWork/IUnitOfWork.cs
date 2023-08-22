using EShop.Application.Repositories.ProductRepository;

namespace EShop.Application.Repositories.UnitOfWork;

public interface IUnitOfWork
{
	IProductReadRepository ReadProducts { get; }
	IProductWriteRepository WriteProducts { get; }
	// Add other repository properties as needed
	Task<int> SaveChangesAsync();
}
