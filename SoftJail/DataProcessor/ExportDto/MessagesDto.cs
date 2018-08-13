using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace SoftJail.DataProcessor
{
    [XmlType("Message")]
    public class MessagesDto
    {
        [Required]
        [XmlElement("Description")]
        public string Description { get; set; }
    }
}
