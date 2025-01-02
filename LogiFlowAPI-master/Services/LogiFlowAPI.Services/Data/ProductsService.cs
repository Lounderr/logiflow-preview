namespace LogiFlowAPI.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using LogiFlowAPI.Common.Extensions;
    using LogiFlowAPI.Data.Common.Repositories;
    using LogiFlowAPI.Data.Models;
    using LogiFlowAPI.Services.Common;
    using LogiFlowAPI.Services.Common.Result;
    using LogiFlowAPI.Services.Interfaces;
    using LogiFlowAPI.Services.Mapping;
    using LogiFlowAPI.Web.Models.Products;

    using Microsoft.EntityFrameworkCore;

    public class ProductsService : DataService, IProductsService
    {
        private readonly IDeletableEntityRepository<Warehouse> warehouseRepo;
        private readonly IDeletableEntityRepository<Product> productRepo;
        private readonly string userId;

        public ProductsService(
            IDeletableEntityRepository<Warehouse> warehouseRepo,
            IAuthenticationContext authenticationContextService,
            IDeletableEntityRepository<Product> productRepo)
        {
            this.warehouseRepo = warehouseRepo;
            this.userId = authenticationContextService.User.Claims.GetId();
            this.productRepo = productRepo;
        }

        public async Task<Result<int>> CreateProductAsync(CreateProductModel model)
        {
            var product = this.Map<Product>(model);

            var isWarehouseAccessible = await this.warehouseRepo
                .AnyAsync(w => w.Id == product.WarehouseId && w.Team.Members.Any(m => m.Id == this.userId));

            if (!isWarehouseAccessible)
            {
                return Result<int>.Error(404, ClientMessages.NotFoundOrInaccessible);
            }

            await this.productRepo.AddAsync(product);
            await this.productRepo.SaveChangesAsync();

            return Result<int>.Ok(200, product.Id);
        }

        public async Task<Result> DeleteProductAsync(int productId)
        {
            var product = await this.productRepo.AllAsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == productId && p.Warehouse.Team.Members.Any(m => m.Id == this.userId));

            if (product == null)
            {
                return Result<int>.Error(404, ClientMessages.NotFoundOrInaccessible);
            }

            this.productRepo.Delete(product);
            await this.productRepo.SaveChangesAsync();

            return Result.Ok(200);
        }

        public async Task<Result<GetProductModel>> GetProductByIdAsync(int productId)
        {
            var product = await this.productRepo
                .AllAsNoTracking()
                .Where(p => p.Id == productId && p.Warehouse.Team.Members.Any(m => m.Id == this.userId))
                .To<GetProductModel>()
                .FirstOrDefaultAsync();

            if (product == null)
            {
                return Result<GetProductModel>.Error(404, ClientMessages.NotFoundOrInaccessible);
            }

            return Result<GetProductModel>.Ok(product);
        }
    }
}
