namespace LogiFlowAPI.Data.Common.Models
{
    using System;

    public abstract class BaseDeletableModel : BaseModel, IDeletableEntity
    {
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
