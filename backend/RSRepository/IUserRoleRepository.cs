using RSData.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RSRepository
{
    public interface IUserRoleRepository
    {
        List<UserRole> GetUserRoles();
        List<UserRole> GetUserRolesByUser(int userId);
        UserRole GetUserRoleById(int id);
        UserRole GetUserRoleByUserAndRole(int userId, int roleId);
        void RemoveUserRole(int userId, int roleId);
        void AddUserRole(UserRole _userrole);
        void UpdateUserRole(UserRole _userrole);
        void DeleteUserRole(UserRole _userrole);
        void SaveChanges();
    }
}
