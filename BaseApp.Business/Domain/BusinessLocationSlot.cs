using BaseApp.Core.Db;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaseApp.Business.Domain
{
    [Table("business_location_slot")]
    public class BusinessLocationSlot : BaseEntity
    {
        [Key]
        [Column("slot_id")]
        public long? SlotId { get; set; }

        [Column("box_id")]
        public long? BoxId { get; set; }

        [Column("location_id")]
        public long? LocationId { get; set; }

        public string? Name { get; set; }

        public string? Code { get; set; }

        [Column("slot_address")]
        public string? SlotAddress { get; set; }
    }
}
