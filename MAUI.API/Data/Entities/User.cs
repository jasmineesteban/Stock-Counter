using System.ComponentModel.DataAnnotations;

namespace MAUI.API.Data.Entities
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        [Required, MaxLength(30)]
        public string Name { get; set; }

        [Required, MaxLength(50)]
        public string Email { get; set; }

        [Required, MaxLength(20)]
        public string Salt { get; set; }

        [Required, MaxLength(180)]
        public string Hash { get; set; }
    }

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

        public virtual ICollection<Item> Options { get; set; }
    }

    public class ItemOption
    {
        public int ItemId { get; set; }

        [Required, MaxLength(50)]
        public string UOM { get; set; }
    }
}
