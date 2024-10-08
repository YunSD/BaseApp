using BaseApp.Core.Db;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaseApp.Business.Domain
{
    [Table("business_goods")]
    public class BusinessGoods : BaseEntity
    {

        [Key]
        [Column("goods_id")]
        public long? GoodsId { get; set; }

        public string? Name { get; set; }

        public string? Code { get; set; }

        public string? Model { get; set; }

        public string? Unit { get; set; }

        public string? Image { get; set; }

    }
}
