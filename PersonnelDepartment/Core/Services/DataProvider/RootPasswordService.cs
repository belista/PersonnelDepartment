﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersonnelDepartment.Models;

namespace PersonnelDepartment.Core.Services.DataProvider
{
    public class RootPasswordService : IRootPasswordService
    {
        private EmployeeContext _db;

        public RootPasswordService(EmployeeContext db)
        {
            _db = db;
        }

        public Task<RootPassword> Get() =>
            Task.Run(() => _db.Passwords.FirstOrDefault());

        public Task<bool> Login(string password) =>
            Task.Run(() => _db.Passwords.FirstOrDefault().Password == password ? true : false);

        public Task Сhange(string oldPassword, string newPassword)
        {
            return Task.Run(() =>
            {
                var password = _db.Passwords.SingleOrDefault(p => p.Password == oldPassword) ?? throw new InvalidOperationException();

                password.Password = newPassword;
                _db.SaveChanges();
            });
        }
    }
}
