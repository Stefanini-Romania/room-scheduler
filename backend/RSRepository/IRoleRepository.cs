using RSData.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RSRepository
{
    interface IRoleRepository
    {
        IEnumerable<Role> GetRoles();
        Role GetRoleById(int id);
        void AddRole(Role role);
        void UpdateRole(Role role);
        void DeleteRole(Role role);
        void SaveChanges();
    }
}
