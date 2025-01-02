namespace LogiFlowAPI.Web.Models.Deliveries
{
    using System.ComponentModel.DataAnnotations;

    using LogiFlowAPI.Data.Models.Enums;

    public class SetDeliveryStatusModel
    {
        public int DeliveryId { get; set; }

        [EnumDataType(typeof(DeliveryStatusOptions))]
        public int StatusId { get; set; }
    }
}
