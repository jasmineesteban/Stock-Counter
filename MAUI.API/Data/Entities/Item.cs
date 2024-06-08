using System.ComponentModel.DataAnnotations;

namespace MAUI.API.Data.Entities
{
    public class Item

    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(45)]
        public string ItemName { get; set; }

        [Required, MaxLength(45)]
        public string Lot { get; set; }

        [Required, MaxLength(45)]
        public string Expiry { get; set; }

        [Required, MaxLength(45)]
        public double InitialCount { get; set; }

        [Required, MaxLength(45)]
        public double QuantityCount { get; set; }

        [Required, MaxLength(45)]
        public string User { get; set; }

        [Required, MaxLength(45)]
        public string InventoryDate { get; set; }

        public virtual ICollection<Item> ItemOptions { get; set; }
    }
}
 