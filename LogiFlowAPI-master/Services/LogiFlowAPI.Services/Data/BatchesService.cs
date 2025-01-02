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
    using LogiFlowAPI.Web.Models.Batches;

    using Microsoft.EntityFrameworkCore;

    public class BatchesService : DataService, IBatchesService
    {
        private readonly IDeletableEntityRepository<Delivery> deliveriesRepo;
        private readonly IDeletableEntityRepository<Batch> batchesRepo;
        private readonly IDeletableEntityRepository<StorageArea> storageAreaRepo;
        private readonly IDeletableEntityRepository<Product> productsRepo;
        private readonly string userId;

        public BatchesService(
            IDeletableEntityRepository<Delivery> deliveriesRepo,
            IAuthenticationContext authenticationContextService,
            IDeletableEntityRepository<Batch> batchesRepo,
            IDeletableEntityRepository<StorageArea> storageAreaRepo,
            IDeletableEntityRepository<Product> productsRepo)
        {
            this.deliveriesRepo = deliveriesRepo;
            this.userId = authenticationContextService.User.Claims.GetId();
            this.batchesRepo = batchesRepo;
            this.storageAreaRepo = storageAreaRepo;
            this.productsRepo = productsRepo;
        }

        public async Task<Result<int>> CreateBatchAsync(CreateBatchModel model)
        {
            var batch = this.Map<Batch>(model);

            var isProductAccessible = await this.productsRepo.AnyAsync(p =>
                p.Id == model.ProductId &&
                p.Warehouse.Team.Members.Any(m => m.Id == this.userId));

            var isDeliveryAccessible = await this.deliveriesRepo.AnyAsync(d =>
                d.Id == model.DeliveryId &&
                d.Warehouse.Team.Members.Any(m => m.Id == this.userId));

            var isStorageAreaAccessible = await this.storageAreaRepo.AnyAsync(sa =>
                sa.Id == model.StorageAreaId &&
                sa.Warehouse.Team.Members.Any(m => m.Id == this.userId));

            if (!isProductAccessible || !isDeliveryAccessible || !isStorageAreaAccessible)
            {
                return Result<int>.Error(404, ClientMessages.NotFoundOrInaccessible);
            }

            await this.batchesRepo.AddAsync(batch);
            await this.batchesRepo.SaveChangesAsync();

            return Result<int>.Ok(200, batch.Id);
        }

        public async Task<Result> DeleteBatchAsync(int batchId)
        {
            var batch = await this.batchesRepo
                .AllAsNoTracking()
                .FirstOrDefaultAsync(b =>
                    b.Id == batchId &&
                    b.Delivery.Warehouse.Team.Members.Any(m => m.Id == this.userId));

            if (batch == null)
            {
                return Result<int>.Error(404, ClientMessages.NotFoundOrInaccessible);
            }

            this.batchesRepo.Delete(batch);
            await this.batchesRepo.SaveChangesAsync();

            return Result.Ok(200);
        }

        public async Task<Result<GetBatchModel>> GetBatchByIdAsync(int batchId)
        {
            var batch = await this.batchesRepo
                .AllAsNoTracking()
                .Where(b => b.Id == batchId && b.StorageArea.Warehouse.Team.Members.Any(m => m.Id == this.userId))
                .To<GetBatchModel>()
                .FirstOrDefaultAsync();

            if (batch == null)
            {
                return Result<GetBatchModel>.Error(404, ClientMessages.NotFoundOrInaccessible);
            }

            return Result<GetBatchModel>.Ok(batch);
        }
    }
}
