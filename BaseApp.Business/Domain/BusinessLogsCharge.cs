using BaseApp.Core.Db;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaseApp.Business.Domain
{

    /// <summary>
    /// 充电日志
    /// </summary>
    [Table("business_logs_charge")]
    public class BusinessLogsCharge : BaseEntity
    {
        [Key]
        [Column("log_id")]
        public long? LogId { get; set; }

        public string? Username { get; set; }

        public string? Name { get; set; }

        [Column("box_info")]
        public string? BoxInfo { get; set; }

        [Column("location_info")]
        public string? LocationInfo { get; set; }

        [Column("location_slot_info")]
        public string? LocationSlotInfo { get; set; }

        public string? time { get; set; }

    }
}
