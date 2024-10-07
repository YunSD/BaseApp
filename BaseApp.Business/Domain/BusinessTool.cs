using BaseApp.Core.Db;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaterialDemo.Domain.Models.Entity
{
    [Table("business_tool")]
    public class BusinessTool : BaseEntity
    {

        [Key]
        [Column("tool_id")]
        public long? ToolId { get; set; }

        public string? Name { get; set; }

        public string? Code { get; set; }

        public string? Model { get; set; }

        public string? Unit { get; set; }

        public string? Image { get; set; }

    }
}
