using System;
using System.Linq;
using System.Threading.Tasks;
using PersonnelDepartment.Models;
using System.Data.Entity;

namespace PersonnelDepartment.Core.Services.DataProvider
{
    public class RootPasswordService : IRootPasswordService
    {
        private EmployeeContext _db;

        public RootPasswordService(EmployeeContext db)
        {
            _db = db;
            _db.Passwords.Load();
        }

        public Task<RootPassword> Get() =>
            Task.Run(() => _db.Passwords.FirstOrDefault());

        public Task<bool> Login(string password) =>
            Task.Run(() => _db.Passwords.FirstOrDefault().Password == password ? true : false);

        public Task<bool> Сhange(string oldPassword, string newPassword)
        {
            return Task.Run(() =>
            {
                var password = _db.Passwords.SingleOrDefault(p => p.Password == oldPassword);

                if (password != null)
                {
                    password.Password = newPassword;
                    _db.SaveChanges();
                    return true;
                }

                return false;
            });
        }
    }
}
