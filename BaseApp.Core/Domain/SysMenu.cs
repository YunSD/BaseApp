using BaseApp.Core.Db;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaseApp.Core.Domain
{
    [Table("sys_menu")]
    public class SysMenu :BaseEntity
    {

        [Key]
        [Column("menu_id")]
        public long? MenuId { get; set; }

        [Column("parent_id")]
        public long? ParentId { get; set; }
        public string? Name { get; set; }
        public string? Icon { get; set; }
        public string? Router { get; set; }
        public MenuPositionEnum Position { get; set; }
        public int? Seq { get; set; }


        public bool isRoot()
        {
            if (ParentId == 0) return true;
            return false;
        }
    }

    public enum MenuPositionEnum
    {
        TOP, BOTTOM
    }


}
