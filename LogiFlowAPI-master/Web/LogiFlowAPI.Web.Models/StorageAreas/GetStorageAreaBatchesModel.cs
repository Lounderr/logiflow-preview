namespace LogiFlowAPI.Web.Models.StorageAreas
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class GetStorageAreaBatchesModel
    {
        [Required]
        public int StorageAreaId { get; set; }

        [Range(1, int.MaxValue)]
        public int Page { get; set; }
    }
}
