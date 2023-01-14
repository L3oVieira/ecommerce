using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace ecom_api.Controllers
{
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _repo;
        public ProductsController(IProductRepository repo)
        {
            _repo = repo;
        }

        [HttpGet("/product")]
        public async Task<ActionResult<List<Product>>> GetProduct()
        {
            var products = await _repo.GetProductsAsync();
            return Ok(products);
        }

        [HttpGet("product/{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _repo.GetProductByIdAsync(id);
            return Ok(product);
        }

        [HttpGet("brands")]
        public async Task<IReadOnlyList<ProductBrand>> GetProductBrandsAsync()
        {
            var products = await _repo.GetProductBrandsAsync();
            return (IReadOnlyList<ProductBrand>)products;
        }

        [HttpGet("types")]
        public async Task<IReadOnlyList<ProductType>> GetProductTypesAsync()
        {
            var products = await _repo.GetProductTypesAsync();
            return (IReadOnlyList<ProductType>)products;
        }
    }
}
