using System;
using System.Collections.Generic;
using System.Text;
using RSData.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace RSRepository
{
    public class UserRoleRepository : IUserRoleRepository
    {

        private RoomPlannerDevContext context;
        private DbSet<UserRole> userroles;

        public UserRoleRepository(RoomPlannerDevContext context)
        {
            this.context = context;
            userroles = context.Set<UserRole>();
        }

        public void AddUserRole(UserRole _userrole)
        {
            if (_userrole == null)
            {
                throw new ArgumentNullException("Add a null UserRole");
            }
            userroles.Add(_userrole);
        }

        public void DeleteUserRole(UserRole _userrole)
        {
            if (_userrole == null)
            {
                throw new ArgumentNullException("Delete null UserRole");
            }
            userroles.Remove(_userrole);
            context.SaveChanges();
        }
        
        public UserRole GetUserRoleById(int id)
        {
            return userroles.FirstOrDefault(s => s.Id == id);
        }

        public List<UserRole> GetUserRoles()
        {
            return userroles.ToList();
        }

        public List<UserRole> GetUserRolesByUser(int userId)
        {
            return userroles.Where(e => e.UserId == userId)
                            .ToList();
        }

        public UserRole GetUserRoleByUserAndRole(int userId, int roleId)
        {
            return userroles.FirstOrDefault(e => e.UserId == userId && e.RoleId == roleId);
        }

        public void RemoveUserRole(int userId, int roleId)
        {
            UserRole userRole = userroles.FirstOrDefault(e => e.UserId == userId && e.RoleId == roleId);
            userroles.Remove(userRole);
        }

        public void UpdateUserRole(UserRole _userrole)
        {
            if (_userrole == null)
            {
                throw new ArgumentNullException("Update a null UserRole");
            }
        }


    }
}
