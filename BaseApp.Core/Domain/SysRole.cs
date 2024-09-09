using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BaseApp.Core.Db;

namespace BaseApp.Core.Domain
{
    [Table("sys_role")]
    public class SysRole : BaseEntity
    {

        [Key]
        [Column("role_id")]
        public long? RoleId { get; set; }

        public string? Name { get; set; }

        public string? Code { get; set; }
    }
}
