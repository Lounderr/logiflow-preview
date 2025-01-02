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
    using LogiFlowAPI.Web.Models.Deliveries;

    using Microsoft.EntityFrameworkCore;

    public class DeliveriesService : DataService, IDeliveriesService
    {
        private readonly IDeletableEntityRepository<Warehouse> warehouseRepo;
        private readonly IDeletableEntityRepository<Delivery> deliveriesRepo;
        private readonly string userId;

        public DeliveriesService(
            IDeletableEntityRepository<Warehouse> warehouseRepo,
            IDeletableEntityRepository<Delivery> deliveriesRepo,
            IAuthenticationContext authenticationContextService)
        {
            this.warehouseRepo = warehouseRepo;
            this.deliveriesRepo = deliveriesRepo;
            this.userId = authenticationContextService.User.Claims.GetId();
        }

        public async Task<Result<int>> CreateDeliveryAsync(CreateDeliveryModel model)
        {
            var delivery = this.Map<Delivery>(model);

            var isWarehouseAccessible = await this.warehouseRepo.AnyAsync(w => w.Id == delivery.WarehouseId && w.Team.OwnerId == this.userId);

            if (!isWarehouseAccessible)
            {
                return Result<int>.Error(404, ClientMessages.NotFoundOrInaccessible);
            }

            delivery.DeliveryStatusId = (int)DeliveryStatusOptions.Pending;

            await this.deliveriesRepo.AddAsync(delivery);
            await this.deliveriesRepo.SaveChangesAsync();

            return Result<int>.Ok(200, delivery.Id);
        }

        public async Task<Result> DeleteDeliveryAsync(int deliveryId)
        {
            var delivery = await this.deliveriesRepo.AllAsNoTracking().FirstOrDefaultAsync(d => d.Id == deliveryId && d.Warehouse.Team.OwnerId == this.userId);

            if (delivery == null)
            {
                return Result<int>.Error(404, ClientMessages.NotFoundOrInaccessible);
            }

            this.deliveriesRepo.Delete(delivery);
            await this.deliveriesRepo.SaveChangesAsync();

            return Result.Ok(200);
        }

        public async Task<Result<GetDeliveryModel>> GetDeliveryByIdAsync(int deliveryId)
        {
            var delivery = await this.deliveriesRepo
                .AllAsNoTracking()
                .Where(d => d.Id == deliveryId && d.Warehouse.Team.Members.Any(m => m.Id == this.userId))
                .To<GetDeliveryModel>()
                .FirstOrDefaultAsync();

            if (delivery == null)
            {
                return Result<GetDeliveryModel>.Error(404, ClientMessages.NotFoundOrInaccessible);
            }

            return Result<GetDeliveryModel>.Ok(delivery);
        }

        public async Task<Result<IEnumerable<BatchSummaryModel>>> GetDeliveryBatchesAsync(GetDeliveryBatchesModel model)
        {
            var delivery = await this.deliveriesRepo
                .AllAsNoTracking()
                .Where(d => d.Id == model.DeliveryId && d.Warehouse.Team.Members.Any(m => m.Id == this.userId))
                .Select(d => new { Batches = d.Batches.Skip(15 * model.Page).Take(15) })
                .FirstOrDefaultAsync();

            if (delivery == null)
            {
                return Result<IEnumerable<BatchSummaryModel>>.Error(404, ClientMessages.NotFoundOrInaccessible);
            }

            var batches = this.Map<IEnumerable<BatchSummaryModel>>(delivery.Batches);

            return Result<IEnumerable<BatchSummaryModel>>.Ok(batches);
        }

        public async Task<Result> SetStatusAsync(SetDeliveryStatusModel model)
        {
            var delivery = await this.deliveriesRepo
                .All()
                .Where(d => d.Id == model.DeliveryId && d.Warehouse.Team.Members.Any(m => m.Id == this.userId))
                .FirstOrDefaultAsync();

            if (delivery == null)
            {
                return Result.Error(404, ClientMessages.NotFoundOrInaccessible);
            }

            delivery.DeliveryStatusId = model.StatusId;

            await this.deliveriesRepo.SaveChangesAsync();

            return Result.Ok();
        }
    }
}
