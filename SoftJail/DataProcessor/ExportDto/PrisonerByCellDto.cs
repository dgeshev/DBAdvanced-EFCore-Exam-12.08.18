using System.Collections.Generic;

namespace SoftJail.DataProcessor.ExportDto
{
    public class PrisonerByCellDto
    {

        public int Id { get; set; }

        public string Name { get; set; }

        public int CellNumber { get; set; }

        public ICollection<OfficersDto> Officers { get; set; }

        public decimal TotalOfficerSalary { get; set; }
    }
}
