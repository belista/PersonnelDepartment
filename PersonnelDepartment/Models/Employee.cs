using System;
using System.ComponentModel;
using System.Diagnostics;

namespace PersonnelDepartment.Models
{
    /// <summary>
    /// Работник.
    /// </summary>
    [DebuggerDisplay("UserName - {FullName}, Id - {Id}")]
    public class Employee : INotifyPropertyChanged
    {
        /// <summary>
        /// Идентификатор работника.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Первая фамилия.
        /// </summary>
        public string FirstSurname { get; set; }

        /// <summary>
        /// Вторая фамилия.
        /// </summary>
        public string SecondSurname { get; set; }

        /// <summary>
        /// Третья фамилия.
        /// </summary>
        public string ThirdSurname { get; set; }

        /// <summary>
        /// Имя.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Отчество.
        /// </summary>
        public string Patronymic { get; set; }

        /// <summary>
        /// Паспортные данные.
        /// </summary>
        public string PassportInfo { get; set; }

        /// <summary>
        /// Прописка.
        /// </summary>
        public string Registration { get; set; }

        /// <summary>
        /// Инн.
        /// </summary>
        public string RegistrationNumber { get; set; }

        /// <summary>
        /// Телефон.
        /// </summary>
        public int Phone { get; set; }

        /// <summary>
        /// Первая должность.
        /// </summary>
        public string FirstPosition { get; set; }

        /// <summary>
        /// Вторая должность.
        /// </summary>
        public string SecondPosition { get; set; }

        /// <summary>
        /// Третья должность.
        /// </summary>
        public string ThirdPosition { get; set; }


        /// <summary>
        /// Первый приказ.
        /// </summary>
        public string FirstOrder { get; set; }

        /// <summary>
        /// Уволен.
        /// </summary>
        public bool Dismissed { get; set; }

        /// <summary>
        /// Второй приказ.
        /// </summary>
        public string SecondOrder { get; set; }

        /// <summary>
        /// Дополнительно.
        /// </summary>
        public string Additionally { get; set; }


        public string FullName => $"{FirstSurname} {Name} {Patronymic}";

        #region Dates
        /// <summary>
        /// Дата увольнения.
        /// </summary>
        public DateTime? DateOfDismissal { get; set; }

        /// <summary>
        /// Дата приема на работу.
        /// </summary>
        public DateTime EmploymentDate { get; set; } = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 1);

        /// <summary>
        /// Дата рождения.
        /// </summary>
        public DateTime DateOfBirth { get; set; } = DateTime.Now;

        /// <summary>
        /// Дни работы.
        /// </summary>
        public double WorkDays => ((DateOfDismissal ?? DateTime.Now) - EmploymentDate).TotalDays;
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
    }
}