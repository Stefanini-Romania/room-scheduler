using System;
using System.Collections.Generic;
using System.Text;
using RSData.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace RSRepository
{
    class RoleRepository : IRoleRepository
    {
        private RoomPlannerDevContext _context;
        private DbSet<Role> _roles;

        public RoleRepository(RoomPlannerDevContext context)
        {
            _context = context;
            _roles = context.Set<Role>();
        }
        
        public void AddRole(Role role)
        {
            if (role == null)
            {
                throw new ArgumentNullException("Add a null role");
            }
            _roles.Add(role);
            _context.SaveChanges();
        }

        public void DeleteRole(Role role)
        {
            if (role == null)
            {
                throw new ArgumentNullException("Delete null role");
            }
            _roles.Remove(role);
            _context.SaveChanges();
        }

        public Role GetRoleById(int id)
        {
            return _roles.SingleOrDefault(s => s.Id == id);
        }

        public List<Role> GetRoles()
        {
            return _roles.ToList();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void UpdateRole(Role role)
        {
            if (role == null)
            {
                throw new ArgumentNullException("Update a null role");
            }
            _context.SaveChanges();
        }
    }
}
