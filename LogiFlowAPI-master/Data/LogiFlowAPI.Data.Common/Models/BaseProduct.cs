namespace LogiFlowAPI.Data.Common.Models
{
    using System.ComponentModel.DataAnnotations;

    // TODO: Might need to move out
    public abstract class BaseProduct : IdentifiableModel<int>
    {
        [StringLength(ModelConstants.BaseProduct.NameMaxLength)]
        public string Name { get; set; }

        // Does not round to 2 decimal places
        [Range(0, double.MaxValue)]
        public double PricePerUnit { get; set; }
    }
}
