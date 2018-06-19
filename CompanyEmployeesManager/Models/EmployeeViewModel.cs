using System;
using System.ComponentModel.DataAnnotations;

namespace CompanyEmployeesManager.Models
{
    public class EmployeeViewModel
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Range(16, 101)]
        public int Age { get; set; }

        [Required]
        [StringLength(100)]
        public string Position { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name="Start date")]
        public DateTime StartDate { get; set; }
    }
}