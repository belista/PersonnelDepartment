using Microsoft.Win32;
using Spire.Xls;
using System;
using System.Data;
using System.Collections.Generic;
using System.Reflection;

namespace PersonnelDepartment.Core.Services.Excel
{
    /// <summary>
    /// Сервис для работы с Excel.
    /// </summary>
    public class ExcelService : IExcelService
    {
        /// <summary>
        /// Путь к файлу.
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// Импорт.
        /// </summary>
        public DataTable OpenExcel()
        {
            OpenFileDialog dlg = new OpenFileDialog
            {
                DefaultExt = ".xls",
                Filter = "Книга Excel|*.xls"
            };

            bool? result = dlg.ShowDialog();

            if (result == true)
            {
                string filename = dlg.FileName;

                Workbook workbook = new Workbook();

                workbook.LoadFromFile(filename);

                Worksheet sheet = workbook.Worksheets[0];

                return sheet.ExportDataTable();
            }

            return null;
        }

        /// <summary>
        /// Экспорт.
        /// </summary>
        /// <param name="dt"></param>
        public void SaveExcel(DataTable dt)
        {
            SaveFileDialog dlg = new SaveFileDialog
            {
                FileName = "Document", // Имя по-умолчанию
                DefaultExt = ".xls", // Расширение по-умолчанию
                Filter = "Книга Excel|*.xls" // Фильтр по расширениям
            };

            // Показываем диалог пользователю
            bool? result = dlg.ShowDialog();

            // Обработка результата работы диалога
            if (result == true)
            {
                // Получаем из диалога полное имя файла
                string filename = dlg.FileName;
                //Данные для записи.
                Workbook book = new Workbook();
                Worksheet sheet = book.Worksheets[0];
                sheet.InsertDataTable(dt, true, 1, 1);
                book.SaveToFile(dlg.FileName);
            }
        }

        /// <summary>
        /// Загружает данные в DataTable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items">Работники.</param>
        /// <returns></returns>
        public DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo prop in Props)
            {
                dataTable.Columns.Add(prop.Name);
            }

            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }

            return dataTable;
        }
    }
}
