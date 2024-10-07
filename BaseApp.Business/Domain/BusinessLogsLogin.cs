using BaseApp.Core.Db;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaseApp.Business.Domain
{

    /// <summary>
    /// 登录日志
    /// </summary>
    [Table("business_logs_login")]
    public class BusinessLogsLogin : BaseEntity
    {
        [Key]
        [Column("log_id")]
        public long? LogId { get; set; }

        public string? Username { get; set; }

        public string? Name { get; set; }

        [Column("login_type")]
        public string? LoginType { get; set; }

    }
}
