using BaseApp.Core.Db;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaseApp.Business.Domain
{

    /// <summary>
    /// 开门日志
    /// </summary>
    [Table("business_logs_open")]
    public class BusinessLogsOpen : BaseEntity
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

    }
}
