using PersonnelDepartment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonnelDepartment.Core.Services.DataProvider
{
    public interface IRootPasswordService
    {
        Task<RootPassword> Get();
        Task<bool> Сhange(string oldPassword, string newPassword);
        Task<bool> Login(string password);
    }
}
