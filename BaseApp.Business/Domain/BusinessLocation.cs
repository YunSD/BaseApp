using BaseApp.Core.Db;
using BaseApp.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseApp.Business.Domain
{
    [Table("business_location")]
    public class BusinessLocation : BaseEntity
    {
        [Key]
        [Column("location_id")]
        public long? LocationId { get; set; }

        public string? Name { get; set; }

        public string? Code { get; set; }

        [Column("light_address")]
        public string? LightAddress { get; set; }

        [Column("lock_address")]
        public string? LockAddress { get; set;}
    }
}
