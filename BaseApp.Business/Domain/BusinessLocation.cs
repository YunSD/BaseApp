using BaseApp.Core.Db;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaseApp.Business.Domain
{
    [Table("business_location")]
    public class BusinessLocation : BaseEntity
    {
        [Key]
        [Column("location_id")]
        public long? LocationId { get; set; }

        [Column("box_id")]
        public long? BoxId { get; set; }

        public string? Name { get; set; }

        public string? Code { get; set; }

        [Column("light_address")]
        public string? LightAddress { get; set; }

        [Column("lock_address")]
        public string? LockAddress { get; set; }
    }
}
