using Microsoft.EntityFrameworkCore;
using RSData.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;

namespace RSRepository
{
    public class UserRepository : IUserRepository
    {
        private RoomPlannerDevContext context;
        private DbSet<User> users;
        private DbSet<UserRole> userrole;
        private DbSet<Role> role;

        public UserRepository(RoomPlannerDevContext context)
        {
            this.context = context;
            users = context.User;
            userrole = context.UserRole;
            role = context.Role;
        }

        public List<User> GetUsers()
        {
            var result = users.Include(u => u.UserRole).Include(u => u.Penalty)
                              .ToList();
            return result;
        }

        public List<User> GetUsers2()
        {
            var result = users.ToList();
            return result;
        }

        public User FindUserByCredential(string email, string password)
        {
            var sha1 = System.Security.Cryptography.SHA1.Create();

            var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(password));
            var encryptedPass = BitConverter.ToString(hash).Replace("-", "").ToLower();

            var user = users.Include(u => u.UserRole).Include(u => u.Penalty).FirstOrDefault(c => c.Email == email && c.Password == encryptedPass);

            return user;
        }

        public User GetUserById(int id)
        {
            return users.Include(u => u.UserRole).FirstOrDefault(s => s.Id == id);
        }

        public User GetUserByEmailAndActive(String email)
        {
            return users.FirstOrDefault(s => (s.Email == email && s.IsActive == true));
        }

        public User GetUserByEmail(String email)
        {
            return users.FirstOrDefault(s => s.Email == email);
        }

        public User GetUserByResetPassCode(String ResetPass)
        {
            return users.FirstOrDefault(s => s.ResetPassCode == ResetPass);
        }

        public List<User>GetUserByisActiv()
        {
            return users.Where(s => s.IsActive != null).ToList();
        }

        public List<User> GetUserByisInactiv()
        {
            return users.Where(s => s.IsActive == false).ToList();
        }

        public List<User> GetUsersByEmail(string email, int userId)
        {
            return users.Where(s => s.Email == email)
                        .Where(s => s.Id != userId)
                        .ToList();
        }

        public List<User> GetActiveHosts()
        {
            return users.Where(u => u.UserRole.Select(li => li.RoleId).Contains((int)UserRoleEnum.host))
                        .Where(u => u.IsActive == true)
                        .ToList();
        }

        public void AddUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("Add a null user");
            }
            users.Add(user);
        }

        public void UpdateUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("Update a null user");
            }
        }

        public void DeleteUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("Delete null user");
            }
            users.Remove(user);
        }

    }
}
