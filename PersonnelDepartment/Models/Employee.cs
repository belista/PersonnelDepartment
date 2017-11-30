using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonnelDepartment.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string FirstSurname { get; set; }
        public string SecondSurname { get; set; }
        public string ThirdSurname { get; set; }
        public string Name { get; set; }
        public string Patronymic { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PassportInfo { get; set; }
        public string Registration { get; set; }
        public string RegistrationNumber { get; set; }
        public int Phone { get; set; }
        public string FirstPosition { get; set; }
        public string SecondPosition { get; set; }
        public string ThirdPosition { get; set; }
        public DateTime EmploymentDate { get; set; }
        public string FirstOrder { get; set; }
        public bool Dismissed { get; set; }
        public DateTime DateOfDismissal { get; set; }
        public string SecondOrder { get; set; }
        public DateTime WorkDays { get; set; }
        public string Additionally { get; set; }
    }
}
