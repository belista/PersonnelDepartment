using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonnelDepartment.Core.Services.Excel
{
    /// <summary>
    /// Интерфейс для ExcelService.
    /// </summary>
    public interface IExcelService
    {
        string FilePath { get; set; }
        void OpenExcel();
        void SaveExcel(DataTable dt);
        DataTable ToDataTable<T>(List<T> items);
    }
}
