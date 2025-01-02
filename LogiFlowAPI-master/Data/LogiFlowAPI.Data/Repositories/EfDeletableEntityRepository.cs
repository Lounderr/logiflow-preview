namespace LogiFlowAPI.Data.Repositories
{
    using System;
    using System.Linq;

    using LogiFlowAPI.Data.Common.Models;
    using LogiFlowAPI.Data.Common.Repositories;

    using Microsoft.EntityFrameworkCore;

    public class EfDeletableEntityRepository<TEntity> : EfRepository<TEntity>, IDeletableEntityRepository<TEntity>
        where TEntity : class, IDeletableEntity
    {
        public EfDeletableEntityRepository(LogiFlowDbContext context)
            : base(context)
        {
        }

        public override IQueryable<TEntity> All()
        {
            return base.All().Where(x => !x.IsDeleted);
        }

        public override IQueryable<TEntity> AllAsNoTracking()
        {
            return base.AllAsNoTracking().Where(x => !x.IsDeleted);
        }

        public IQueryable<TEntity> AllWithDeleted()
        {
            return base.All().IgnoreQueryFilters();
        }

        public IQueryable<TEntity> AllAsNoTrackingWithDeleted()
        {
            return base.AllAsNoTracking().IgnoreQueryFilters();
        }

        public void HardDelete(TEntity entity)
        {
            base.Delete(entity);
        }

        public void Undelete(TEntity entity)
        {
            entity.IsDeleted = false;
            entity.DeletedOn = null;
            this.Update(entity);
        }

        public override void Delete(TEntity entity)
        {
            entity.IsDeleted = true;
            entity.DeletedOn = DateTime.UtcNow;
            this.Update(entity);
        }
    }
}
