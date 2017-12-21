using PersonnelDepartment.Models;
using System.Collections.Generic;
using System.Data;
using System.Printing;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PersonnelDepartment.Core.Services.Printing
{
    public static class PrintService
    {
        public static void Print(List<Employee> employees)
        {
            var printDialog = new PrintDialog();

            var prntkt = printDialog.PrintTicket;
            prntkt.PageOrientation = PageOrientation.Landscape;
            printDialog.PrintTicket = prntkt;

            if (printDialog.ShowDialog() == true)
            {
                var paginator = new StoreDataSetPaginator(employees.ToDataTable(),
                    new Typeface("Calibri"), 24, 48 * 0.75,
                    new Size(printDialog.PrintableAreaWidth, printDialog.PrintableAreaHeight),
                    true);

                printDialog.PrintDocument(paginator, "Печать с помощью классов визуального уровня");
            }
        }

        public static void PrintEmployee(List<Employee> employees)
        {
            var printDialog = new PrintDialog();

            var prntkt = printDialog.PrintTicket;
            prntkt.PageOrientation = PageOrientation.Portrait;
            printDialog.PrintTicket = prntkt;

            if (printDialog.ShowDialog() == true)
            {
                var paginator = new StoreDataSetPaginator(employees.ToDataTable(),
                    new Typeface("Calibri"), 24, 48 * 0.75,
                    new Size(printDialog.PrintableAreaWidth, printDialog.PrintableAreaHeight),
                    false);

                printDialog.PrintDocument(paginator, "Печать с помощью классов визуального уровня");
            }
        }

        public static DataTable ToDataTable<T>(this List<T> collection)
        {
            var dataTable = new DataTable(typeof(T).Name);

            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                dataTable.Columns.Add(property.Name);
            }

            foreach (var item in collection)
            {
                var values = new object[properties.Length];
                for (var i = 0; i < properties.Length; i++)
                {
                    values[i] = properties[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }

            return dataTable;
        }
    }
}
