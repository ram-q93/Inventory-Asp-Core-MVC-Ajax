using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory_Asp_Core_MVC_Ajax.EFModels
{
    [Table("Images")]
    public class Image
    {
        public int Id { get; set; }

        [MaxLength(50)]
        [Column(TypeName = "varchar(50)")]
        public string Title { get; set; }

        [MaxLength(150)]
        [Column(TypeName = "varchar(150)")]
        public string Caption { get; set; }

        public byte[] Data { get; set; }

        public int productId { get; set; }
        public Product product { get; set; }
    }
}
