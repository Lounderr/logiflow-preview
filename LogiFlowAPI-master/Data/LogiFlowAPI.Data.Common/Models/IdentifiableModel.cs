namespace LogiFlowAPI.Data.Common.Models
{
    using System.ComponentModel.DataAnnotations;

    public abstract class IdentifiableModel<TKey> : BaseDeletableModel
    {
        [Key]
        public TKey Id { get; set; }
    }
}
