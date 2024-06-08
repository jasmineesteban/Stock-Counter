using System.ComponentModel.DataAnnotations;

namespace MAUI.API.Data.Entities
{
    public class Sheet
    {
        [Key]

        public long Id { get; set; }

        [Required]
        public long StockCountSheetId { get; set; }

        public DateTime CountAt { get; set; } = DateTime.Now;

        public Guid EmployeeId { get; set; }

        [Required, MaxLength(30)]

        public string EmployeeName { get; set; }

        public virtual ICollection<CountItems> CountItems { get; set; }
    }
}
 