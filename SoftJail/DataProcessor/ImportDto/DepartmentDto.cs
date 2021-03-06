﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SoftJail.ImportResults
{
    public class DepartmentDto
    {
        [Required]
        [StringLength(25, MinimumLength = 3)]
        public string Name { get; set; }

        public ICollection<CellDto> Cells { get; set; }
    }
}
