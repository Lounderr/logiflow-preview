namespace LogiFlowAPI.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using LogiFlowAPI.Common.Extensions;
    using LogiFlowAPI.Data.Common.Repositories;
    using LogiFlowAPI.Data.Models;
    using LogiFlowAPI.Data.Models.Enums;
    using LogiFlowAPI.Services.Common;
    using LogiFlowAPI.Services.Common.Result;
    using LogiFlowAPI.Services.Interfaces;
    using LogiFlowAPI.Services.Mapping;
    using LogiFlowAPI.Web.Models.Batches;
    using LogiFlowAPI.Web.Models.StorageAreas;

    using Microsoft.EntityFrameworkCore;

    public class StorageAreasService : DataService, IStorageAreasService
    {
        private readonly IDeletableEntityRepository<StorageArea> storageAreaRepo;
        private readonly IDeletableEntityRepository<Warehouse> warehouseRepo;
        private readonly string userId;

        public StorageAreasService(
            IDeletableEntityRepository<StorageArea> storageAreaRepo,
            IAuthenticationContext authenticationContextService,
            IDeletableEntityRepository<Warehouse> warehouseRepo)
        {
            this.storageAreaRepo = storageAreaRepo;
            this.userId = authenticationContextService.User.Claims.GetId();
            this.warehouseRepo = warehouseRepo;
        }

        public async Task<Result<int>> CreateStorageAreaAsync(CreateStorageAreaModel model)
        {
            var storageArea = this.Map<StorageArea>(model);

            var isWarehouseAccessible = await this.warehouseRepo.AnyAsync(w => w.Id == storageArea.WarehouseId && w.Team.OwnerId == this.userId);

            if (!isWarehouseAccessible)
            {
                return Result<int>.Error(404, ClientMessages.NotFoundOrInaccessible);
            }

            await this.storageAreaRepo.AddAsync(storageArea);
            await this.storageAreaRepo.SaveChangesAsync();

            return Result<int>.Ok(200, storageArea.Id);
        }

        public async Task<Result> DeleteStorageAreaAsync(int storageAreaId)
        {
            var storageArea = await this.storageAreaRepo.AllAsNoTracking().FirstOrDefaultAsync(sa => sa.Id == storageAreaId && sa.Warehouse.Team.OwnerId == this.userId);

            if (storageArea == null)
            {
                return Result<int>.Error(404, ClientMessages.NotFoundOrInaccessible);
            }

            this.storageAreaRepo.Delete(storageArea);
            await this.storageAreaRepo.SaveChangesAsync();

            return Result.Ok(200);
        }

        public async Task<Result<GetStorageAreaModel>> GetStorageAreaByIdAsync(int storageAreaId)
        {
            var storageArea = await this.storageAreaRepo
                .AllAsNoTracking()
                .Where(sa => sa.Id == storageAreaId && sa.Warehouse.Team.Members.Any(m => m.Id == this.userId))
                .To<GetStorageAreaModel>()
                .FirstOrDefaultAsync();

            if (storageArea == null)
            {
                return Result<GetStorageAreaModel>.Error(404, ClientMessages.NotFoundOrInaccessible);
            }

            return Result<GetStorageAreaModel>.Ok(storageArea);
        }

        public async Task<Result<IEnumerable<BatchSummaryModel>>> GetStorageAreaBatchesAsync(GetStorageAreaBatchesModel model)
        {
            var storageArea = await this.storageAreaRepo
                .AllAsNoTracking()
                .Where(sa => sa.Id == model.StorageAreaId && sa.Warehouse.Team.Members.Any(m => m.Id == this.userId))
                .Select(w => new
                {
                    Batches = w.Batches
                        .Where(b => b.Delivery.DeliveryStatusId == (int)DeliveryStatusOptions.Completed)
                        .Skip(15 * model.Page)
                        .Take(15)
                })
                .FirstOrDefaultAsync();

            if (storageArea == null)
            {
                return Result<IEnumerable<BatchSummaryModel>>.Error(404, ClientMessages.NotFoundOrInaccessible);
            }

            var batches = this.Map<IEnumerable<BatchSummaryModel>>(storageArea.Batches);

            return Result<IEnumerable<BatchSummaryModel>>.Ok(batches);
        }
    }
}
