using System.ComponentModel.DataAnnotations;

namespace MAUI.API.Data.Entities
{
    public class ItemOption
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ItemId { get; set; }

        [Required, MaxLength(50)]
        public string UOM { get; set; }

        public virtual Item Item { get; set; }
    }
}
 