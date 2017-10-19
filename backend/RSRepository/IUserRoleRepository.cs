﻿using RSData.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RSRepository
{
    public interface IUserRoleRepository
    {
        IEnumerable<UserRole> GetUserRoles();
        UserRole GetUserRoleById(int id);
        void AddUserRole(UserRole _userrole);
        void UpdateUserRole(UserRole _userrole);
        void DeleteUserRole(UserRole _userrole);
        void SaveChanges();
    }
}
