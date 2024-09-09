using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BaseApp.Core.Db;

namespace BaseApp.Core.Domain
{

    [Table("sys_role_menu")]
    public class SysRoleMenu : BaseEntity
    {
        [Key]
        public long? Id { get; set; }

        [Column("role_id")]
        public long? RoleId { get; set; }

        [Column("menu_id")]
        public long? MenuId { get; set; }
    }
}
