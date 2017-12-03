using System;
using System.ComponentModel;
using System.Diagnostics;

namespace PersonnelDepartment.Models
{
    /// <summary>
    /// 
    /// </summary>
    [DebuggerDisplay("UserName - {FullName}, Id - {Id}")]
    public class Employee : INotifyPropertyChanged
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string FirstSurname { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SecondSurname { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ThirdSurname { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Patronymic { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PassportInfo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Registration { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string RegistrationNumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Phone { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string FirstPosition { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SecondPosition { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ThirdPosition { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public string FirstOrder { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool Dismissed { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SecondOrder { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Additionally { get; set; }


        public string FullName => $"{FirstSurname} {Name} {Patronymic}";

        #region Dates
        /// <summary>
        /// 
        /// </summary>
        public DateTime? DateOfDismissal { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime EmploymentDate { get; set; } = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 1);

        /// <summary>
        /// 
        /// </summary>
        public DateTime DateOfBirth { get; set; } = DateTime.Now;

        /// <summary>
        /// 
        /// </summary>
        public double WorkDays => ((DateOfDismissal ?? DateTime.Now) - EmploymentDate).TotalDays;
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
    }
}