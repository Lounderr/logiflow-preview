namespace LogiFlowAPI.Services.Interfaces
{
    using System.Threading.Tasks;

    using LogiFlowAPI.Services.Common.Result;
    using LogiFlowAPI.Web.Models.Products;

    public interface IProductsService
    {
        Task<Result<int>> CreateProductAsync(CreateProductModel model);
        Task<Result> DeleteProductAsync(int productId);
        Task<Result<GetProductModel>> GetProductByIdAsync(int productId);
    }
}