using Microsoft.Win32;
using Spire.Xls;
using System;
using System.Data;
using System.Collections.Generic;
using System.Reflection;

namespace PersonnelDepartment.Core.Services.Excel
{
    public class ExcelService : IExcelService
    {
        public string FilePath { get; set; }

        public void OpenExcel()
        {
            throw new NotImplementedException();
        }

        public void SaveExcel(DataTable dt)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.FileName = "Document"; // Имя по-умолчанию
            dlg.DefaultExt = ".xls"; // Расширение по-умолчанию
            dlg.Filter = "Книга Excel|*.xls"; // Фильтр по расширениям

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
