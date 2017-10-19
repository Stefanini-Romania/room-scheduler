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
            context.SaveChanges();
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
            return userroles.SingleOrDefault(s => s.Id == id);
        }

        public IEnumerable<UserRole> GetUserRoles()
        {
            return userroles.AsEnumerable();
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }

        public void UpdateUserRole(UserRole _userrole)
        {
            if (_userrole == null)
            {
                throw new ArgumentNullException("Update a null UserRole");
            }
            context.SaveChanges();
        }


    }
}
