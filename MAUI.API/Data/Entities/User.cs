using System.ComponentModel.DataAnnotations;

namespace MAUI.API.Data.Entities
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        [Required, MaxLength(30)]
        public string Name { get; set; }

        [Required, MaxLength(20)]
        public string Salt { get; set; }

        [Required, MaxLength(180)]
        public string Hash { get; set; }
    }
}
 