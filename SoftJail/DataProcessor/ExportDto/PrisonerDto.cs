using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace SoftJail.DataProcessor.ExportDto
{
    [XmlType("Prisoner")]
    public class PrisonerDto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3)]
        [XmlElement("Name")]
        public string Name { get; set; }


        [Required]
        [XmlElement("IncarcerationDate")]
        public string IncarcerationDate { get; set; }

        [XmlArray("EncryptedMessages")]
        public MessagesDto[] Mail { get; set; }
    }
}
