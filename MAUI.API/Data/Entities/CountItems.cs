using System.ComponentModel.DataAnnotations;

namespace MAUI.API.Data.Entities
{
    public class CountItems
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public long CountSheetId { get; set; }

        [Required]
        public int ItemId { get; set; }

        [Required, MaxLength(50)]
        public string EmployeeName { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Required, MaxLength(50)]
        public string UOM { get; set; }

        public virtual Item Item { get; set; }

    }
}
 