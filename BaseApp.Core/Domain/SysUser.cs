using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BaseApp.Core.Db;
using BaseApp.Core.Enums;

namespace BaseApp.Core.Domain
{
    [Table("sys_user")]
    public class SysUser : BaseEntity
    {

        [Key]
        [Column("user_id")]
        public long? UserId { get; set; }

        [Column("role_id")]
        public long? RoleId { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Avatar { get; set; }

        [Column("info_card")]
        public string? InfoCard { get; set; }

        [Column("info_face")]
        public string? InfoFace { get; set; }

        [Column("info_touch")]
        public string? InfoTouch { get; set; }

        [Column("lock_flag")]
        public BaseStatusEnum LockFlag { get; set; }


        public bool IsLocked() { return BaseStatusEnum.EXCEPTION.Equals(LockFlag); }
    }
}
