using BaseApp.Core.Db;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaseApp.Business.Domain
{
    [Table("business_box")]
    public class BusinessBox : BaseEntity
    {
        [Key]
        [Column("box_id")]
        public long? BoxId { get; set; }

        public string? Name { get; set; }

        public string? Code { get; set; }

        [Column("light_control_address")]
        public string? LightControlAddress { get; set; }

        [Column("lock_control_address")]
        public string? LockControlAddress { get; set; }


    }
}
