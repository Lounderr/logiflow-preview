namespace LogiFlowAPI.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using LogiFlowAPI.Common.Extensions;
    using LogiFlowAPI.Data.Common.Repositories;
    using LogiFlowAPI.Data.Models;
    using LogiFlowAPI.Services.Common;
    using LogiFlowAPI.Services.Common.Result;
    using LogiFlowAPI.Services.Interfaces;
    using LogiFlowAPI.Services.Mapping;
    using LogiFlowAPI.Web.Models.Deliveries;
    using LogiFlowAPI.Web.Models.StorageAreas;
    using LogiFlowAPI.Web.Models.Warehouses;

    using Microsoft.EntityFrameworkCore;

    public class WarehousesService : DataService, IWarehousesService
    {
        private readonly IDeletableEntityRepository<Warehouse> warehousesRepo;
        private readonly string userId;
        private readonly IDeletableEntityRepository<Team> teamsRepo;

        public WarehousesService(
            IDeletableEntityRepository<Warehouse> warehousesRepo,
            IAuthenticationContext authenticationContextService,
            IDeletableEntityRepository<Team> teamsRepo)
        {
            this.warehousesRepo = warehousesRepo;
            this.userId = authenticationContextService.User.Claims.GetId();
            this.teamsRepo = teamsRepo;
        }

        public async Task<Result<int>> CreateWarehouseAsync(CreateWarehouseModel model)
        {
            var warehouse = this.Map<Warehouse>(model);

            var isTeamAccessible = await this.teamsRepo.AnyAsync(t => t.Id == model.TeamId && t.OwnerId == this.userId);

            if (!isTeamAccessible)
            {
                return Result<int>.Error(404, ClientMessages.NotFoundOrInaccessible);
            }

            await this.warehousesRepo.AddAsync(warehouse);
            await this.warehousesRepo.SaveChangesAsync();

            return Result<int>.Ok(200, warehouse.Id);
        }

        public async Task<Result> DeleteWarehouseAsync(int warehouseId)
        {
            var warehouse = await this.warehousesRepo
                .AllAsNoTracking()
                .FirstOrDefaultAsync(w => w.Id == warehouseId && w.Team.OwnerId == this.userId);

            if (warehouse == null)
            {
                return Result<int>.Error(404, ClientMessages.NotFoundOrInaccessible);
            }

            this.warehousesRepo.Delete(warehouse);
            await this.warehousesRepo.SaveChangesAsync();

            return Result.Ok(200);
        }

        public async Task<Result<GetWarehouseModel>> GetWarehouseByIdAsync(int warehouseId)
        {
            var warehouse = await this.warehousesRepo
                .AllAsNoTracking()
                .Where(w => w.Id == warehouseId && w.Team.Members.Any(m => m.Id == this.userId))
                .To<GetWarehouseModel>()
                .FirstOrDefaultAsync();

            if (warehouse == null)
            {
                return Result<GetWarehouseModel>.Error(404, ClientMessages.NotFoundOrInaccessible);
            }

            return Result<GetWarehouseModel>.Ok(warehouse);
        }

        public async Task<Result<IEnumerable<StorageAreaSummaryModel>>> GetWarehouseStorageAreasAsync(int warehouseId)
        {
            var warehouse = await this.warehousesRepo
                .AllAsNoTracking()
                .Where(w => w.Id == warehouseId && w.Team.Members.Any(m => m.Id == this.userId))
                .Select(w => new { w.StorageAreas })
                .FirstOrDefaultAsync();

            if (warehouse == null)
            {
                return Result<IEnumerable<StorageAreaSummaryModel>>.Error(404, ClientMessages.NotFoundOrInaccessible);
            }

            var storageAreas = this.Map<IEnumerable<StorageAreaSummaryModel>>(warehouse.StorageAreas);

            return Result<IEnumerable<StorageAreaSummaryModel>>.Ok(storageAreas);
        }

        public async Task<Result<IEnumerable<DeliverySummaryModel>>> GetWarehouseDeliveriesAsync(GetWarehouseDeliveriesModel model)
        {
            var warehouse = await this.warehousesRepo
                .AllAsNoTracking()
                .Where(w => w.Id == model.WarehouseId && w.Team.Members.Any(m => m.Id == this.userId))
                .Select(w => new { Deliveries = w.Deliveries.Skip(15 * model.Page).Take(15) })
                .FirstOrDefaultAsync();

            if (warehouse == null)
            {
                return Result<IEnumerable<DeliverySummaryModel>>.Error(404, ClientMessages.NotFoundOrInaccessible);
            }

            var deliveries = this.Map<IEnumerable<DeliverySummaryModel>>(warehouse.Deliveries);

            return Result<IEnumerable<DeliverySummaryModel>>.Ok(deliveries);
        }
    }
}
