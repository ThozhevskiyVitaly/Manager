using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Models
{
    public class Employee
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Range(16, 101)]
        public int Age { get; set; }

        [Required]
        public string Position { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        public override string ToString()
        {
            return $"Employee(Id: {Id}, Name: {Name}, Age: {Age}, Position: {Position}, StartDate: {StartDate})";
        }
    }
}
