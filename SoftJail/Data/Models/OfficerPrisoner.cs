using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoftJail.Data.Models
{
    [Table("OfficersPrisoners")]
    public class OfficerPrisoner
    {
        public int PrisonerId { get; set; }
        [Required]
        public Prisoner Prisoner { get; set; }

        public int OfficerId { get; set; }
        [Required]
        public Officer Officer { get; set; }
    }
}
