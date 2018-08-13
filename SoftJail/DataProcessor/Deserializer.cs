namespace SoftJail.DataProcessor
{

    using Data;
    using Newtonsoft.Json;
    using SoftJail.Data.Models;
    using SoftJail.Data.Models.Enums;
    using SoftJail.DataProcessor.ImportDto;
    using SoftJail.ImportResults;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    public class Deserializer
    {
        public static string ImportDepartmentsCells(SoftJailDbContext context, string jsonString)
        {
            var deserializeDepartmentCells = JsonConvert.DeserializeObject<DepartmentDto[]>(jsonString);

            var sb = new StringBuilder();

            var departments = new List<Department>();
            var cells = new List<Cell>();

            foreach (var depDto in deserializeDepartmentCells)
            {

                if (!isValid(depDto))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                var isCellValid = true;

                foreach (var cellDto in depDto.Cells)
                {
                    if (!isValid(cellDto))
                    {
                        sb.AppendLine("Invalid Data");
                        isCellValid = false;
                        break;

                    }
                }

                if (!isCellValid)
                {
                    continue;
                }

                var department = new Department
                {
                    Name = depDto.Name
                };

                var count = 0;

                foreach (var cellDto in depDto.Cells)
                {
                    var cell = new Cell
                    {
                        CellNumber = cellDto.CellNumber,
                        HasWindow = cellDto.HasWindow,
                        Department = department
                    };

                    count++;
                    cells.Add(cell);
                }

                departments.Add(department);
                sb.AppendLine($"Imported {department.Name} with {count} cells");
                count = 0;
            }

            context.Departments.AddRange(departments);
            context.Cells.AddRange(cells);
            context.SaveChanges();

            return sb.ToString();
        }

        public static string ImportPrisonersMails(SoftJailDbContext context, string jsonString)
        {
            var deserializePrisonerMails = JsonConvert.DeserializeObject<PrisonerDto[]>(jsonString);

            var sb = new StringBuilder();

            var prisoners = new List<Prisoner>();
            var mails = new List<Mail>();

            foreach (var prisonerDto in deserializePrisonerMails)
            {
                if (!isValid(prisonerDto))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                bool isMailValid = true;

                foreach (var mailDto in prisonerDto.Mails)
                {
                    if (!isValid(mailDto))
                    {
                        sb.AppendLine("Invalid Data");
                        isMailValid = false;
                        break;
                    }
                }

                if (!isMailValid)
                {
                    continue;
                }

                var icarcerationDateParse = DateTime.ParseExact(prisonerDto.IncarcerationDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                var prisoner = new Prisoner
                {
                    FullName = prisonerDto.FullName,
                    Nickname = prisonerDto.Nickname,
                    Age = prisonerDto.Age,
                    IncarcerationDate = icarcerationDateParse,
                    Bail = prisonerDto.Bail

                };

                if (prisonerDto.ReleaseDate != null)
                {
                    var releaseDateParse = DateTime.ParseExact(prisonerDto.ReleaseDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    prisoner.ReleaseDate = releaseDateParse;
                }

                prisoners.Add(prisoner);

                foreach (var mailDto in prisonerDto.Mails)
                {
                    var mail = new Mail
                    {
                        Description = mailDto.Description,
                        Sender = mailDto.Sender,
                        Address = mailDto.Sender,
                        Prisoner = prisoner
                    };

                    mails.Add(mail);
                }

                sb.AppendLine($"Imported {prisoner.FullName} {prisoner.Age} years old");
            }

            context.Prisoners.AddRange(prisoners);
            context.Mails.AddRange(mails);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportOfficersPrisoners(SoftJailDbContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(OfficerDto[]), new XmlRootAttribute("Officers"));
            var deserializeOfficers = (OfficerDto[])serializer.Deserialize(new StringReader(xmlString));
            var officers = new List<Officer>();
            var officersPrisoners = new List<OfficerPrisoner>();
            var sb = new StringBuilder();

            foreach (var officerDto in deserializeOfficers)
            {
                if (!isValid(officerDto))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                Position positionValue;
                Weapon weaponValue;

                if (!(Enum.TryParse(officerDto.Position, true, out positionValue))
                    || !(Enum.TryParse(officerDto.Weapon, true, out weaponValue)
                    ))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                var officer = new Officer
                {
                    FullName = officerDto.FullName,
                    Salary = officerDto.Salary,
                    Position = positionValue,
                    Weapon = weaponValue,
                    DepartmentId = officerDto.DepartmentId
                };

                officers.Add(officer);
                var count = 0;

                foreach (var prisonerDto in officerDto.Prisoners)
                {
                    var prisoner = context.Prisoners
                        .FirstOrDefault(x => x.Id == prisonerDto.PrisonerId);

                    count++;
                    var offcerPrisoner = new OfficerPrisoner
                    {
                        OfficerId = officer.Id,
                        PrisonerId = prisoner.Id
                    };

                    officersPrisoners.Add(offcerPrisoner);
                }

                sb.AppendLine($"Imported {officer.FullName} ({count} prisoners)");
                count = 0;
            }
            context.Officers.AddRange(officers);
            context.OfficersPrisoners.AddRange(officersPrisoners);
            //context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static bool isValid(object obj)
        {
            var validationContext = new ValidationContext(obj);

            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(obj, validationContext, validationResult, true);
        }
    }
}