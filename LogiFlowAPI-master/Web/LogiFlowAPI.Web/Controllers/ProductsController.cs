namespace LogiFlowAPI.Web.Controllers
{
    using System.Threading.Tasks;

    using LogiFlowAPI.Services.Interfaces;
    using LogiFlowAPI.Web.Infrastructure.Extensions;
    using LogiFlowAPI.Web.Models.Products;

    using Microsoft.AspNetCore.Mvc;

    [Route("api/products")]
    public class ProductsController : ProtectedController
    {
        private readonly IProductsService productsService;

        public ProductsController(IProductsService productsService)
        {
            this.productsService = productsService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProductAsync(CreateProductModel model)
        {
            return (await this.productsService.CreateProductAsync(model)).ToActionResult();
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> DeleteProductAsync(int productId)
        {
            return (await this.productsService.DeleteProductAsync(productId)).ToActionResult();
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> GetProductByIdAsync(int productId)
        {
            return (await this.productsService.GetProductByIdAsync(productId)).ToActionResult();
        }
    }
}
