namespace SoftJail.DataProcessor
{
    using Data;
    using Newtonsoft.Json;
    using SoftJail.DataProcessor.ExportDto;
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;

    public class Serializer
    {
        public static string ExportPrisonersByCells(SoftJailDbContext context, int[] ids)
        {
            var prisoners = context.Prisoners
                 .Where(x => ids.Any(i => i == x.Id))
                 .Select(x => new PrisonerByCellDto
                 {
                     Id = x.Id,
                     Name = x.FullName,
                     CellNumber = x.Cell.CellNumber,
                     Officers = x.PrisonerOfficers.Select(o => new OfficersDto
                     {
                         OfficerName = o.Officer.FullName,
                         Department = o.Officer.Department.Name
                     })
                     .OrderBy(n => n.OfficerName)
                     .ToArray(),

                     TotalOfficerSalary = x.PrisonerOfficers.Sum(s => s.Officer.Salary)
                 })
                 .OrderBy(x => x.Name)
                 .ThenBy(x => x.Id)
                 .ToArray();

            var jsonString = JsonConvert.SerializeObject(prisoners, Newtonsoft.Json.Formatting.Indented);

            return jsonString;

        }

        public static string ExportPrisonersInbox(SoftJailDbContext context, string prisonersNames)
        {
            var splitNames = prisonersNames.Split(',');

            var prisoners = context.Prisoners
                .Where(x => splitNames.Any(p => p == x.FullName))
                .Select(x => new PrisonerDto
                {
                    Id = x.Id,
                    Name = x.FullName,
                    IncarcerationDate = x.IncarcerationDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                    Mail = x.Mails.Select(m => new MessagesDto
                    {
                        Description = m.Description
                    })
                    .ToArray()
                })
                .OrderBy(p => p.Name)
                .ThenBy(p => p.Id)
                .ToArray();

            var sb = new StringBuilder();
            var xmlSpaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            var serizlizer = new XmlSerializer(typeof(PrisonerDto[]), new XmlRootAttribute("Prisoners"));

            serizlizer.Serialize(new StringWriter(sb), prisoners, xmlSpaces);

            return sb.ToString();
        }
    }
}