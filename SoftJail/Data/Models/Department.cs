using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoftJail.Data.Models
{
    [Table("Departments")]
    public class Department
    {
        public Department()
        {
            this.Cells = new List<Cell>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(25, MinimumLength = 3)]
        public string Name { get; set; }

        public ICollection<Cell> Cells { get; set; }
    }
}
