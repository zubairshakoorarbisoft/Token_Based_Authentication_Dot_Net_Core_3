using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RestFullAPI.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public DateTime JoiningDate { get; set; }
        public decimal Salary { get; set; }
        [Required]
        public string Designation { get; set; }

    }
}
