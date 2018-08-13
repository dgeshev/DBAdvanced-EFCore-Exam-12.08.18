using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoftJail.Data.Models
{
    [Table("Cells")]
    public class Cell
    {
        public Cell()
        {
            this.Prisoners = new List<Prisoner>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [Range(1, 1000)]
        public int CellNumber { get; set; }

        [Required]
        public bool HasWindow { get; set; }

        public int DepartmentId { get; set; }
        [Required]
        public Department Department { get; set; }

        public ICollection<Prisoner> Prisoners { get; set; }
    }
}
