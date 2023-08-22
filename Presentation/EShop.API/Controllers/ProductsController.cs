using EShop.Application.Paginations;
using EShop.Application.Repositories.ProductRepository;
using EShop.Application.Repositories.UnitOfWork;
using EShop.Application.ViewModels;
using EShop.Domain.Entities;
using EShop.Persistence.Repositories.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
	public class ProductsController : ControllerBase
	{
		private readonly IUnitOfWork _unitOfWork;

		public ProductsController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		[HttpGet("getall")]
		public IActionResult GetAll([FromQuery] Pagination pagination)
		{
			try
			{
				var products = _unitOfWork.ReadProducts.GetAll(tracking: false);
				var totalCount = products.Count();

				products = products.OrderBy(p => p.CreatedTime)
								   .Skip(pagination.Size * pagination.Page)
								   .Take(pagination.Size)
								   .ToList();

				return Ok(new { products, totalCount });
			}
			catch (Exception)
			{
				// logging
				return StatusCode((int)HttpStatusCode.InternalServerError);
			}
		}

		[HttpGet("{productId}")]
		public async Task<IActionResult> Get(Guid productId)
		{
			try
			{
				var product = await _unitOfWork.ReadProducts.GetAsync(productId.ToString());

				if (product == null)
				{
					return NotFound();
				}

				return Ok(product);
			}
			catch (Exception)
			{
				// logging
				return StatusCode((int)HttpStatusCode.InternalServerError);
			}
		}

		[HttpPost("add")]
		public async Task<IActionResult> Add([FromBody] AddProductViewModel model)
		{
			try
			{
				if (ModelState.IsValid)
				{
					Product product = new()
					{
						Id = Guid.NewGuid(),
						Name = model.Name,
						Description = model.Desc,
						Price = model.Price,
						Stock = model.Stock,
						CreatedTime = DateTime.Now,
					};

					await _unitOfWork.WriteProducts.AddAsync(product);
					await _unitOfWork.SaveChangesAsync();

					return StatusCode((int)HttpStatusCode.Created);
				}
				return BadRequest(ModelState);
			}
			catch (Exception)
			{
				// logging
				return StatusCode((int)HttpStatusCode.InternalServerError);
			}
		}

		[HttpPost("remove")]
		public async Task<IActionResult> Remove([FromBody] Guid productId)
		{
			try
			{
				var product = await _unitOfWork.ReadProducts.GetAsync(productId.ToString());
				if (product == null)
				{
					return NotFound();
				}

				_unitOfWork.WriteProducts.Remove(product);
				await _unitOfWork.SaveChangesAsync();

				return NoContent();
			}
			catch (Exception)
			{
				// logging
				return StatusCode((int)HttpStatusCode.InternalServerError);
			}
		}

		[HttpPut("update")]
		public async Task<IActionResult> Update([FromBody] UpdateProductViewModel model)
		{
			try
			{
				if (ModelState.IsValid)
				{
					var product = await _unitOfWork.ReadProducts.GetAsync(model.Id.ToString());
					if (product == null)
					{
						return NotFound();
					}

					product.Name = model.Name;
					product.Description = model.Desc;
					product.Price = model.Price;
					product.Stock = model.Stock;

					_unitOfWork.WriteProducts.Update(product);
					await _unitOfWork.SaveChangesAsync();

					return NoContent();
				}
				return BadRequest(ModelState);
			}
			catch (Exception)
			{
				// logging
				return StatusCode((int)HttpStatusCode.InternalServerError);
			}
		}
	}

}

