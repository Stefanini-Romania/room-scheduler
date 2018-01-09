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

        public void SaveChanges()
        {
            context.SaveChanges();
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

        public User FindUserByCredential(string username, string password)
        {
            // TODO:  Nu trebuie sa-i aducem pe toti cu GetUsers. Trebuie cautat direct in DbSet-ul de users  !!

            var sha1 = System.Security.Cryptography.SHA1.Create();

            var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(password));
            var encryptedPass = BitConverter.ToString(hash).Replace("-", "").ToLower();
            var returnvar = this.GetUsers().FirstOrDefault(c => c.Name == username && c.Password == encryptedPass);
            return returnvar;
        }

        public User GetUserById(int id)
        {
            return users.Include(u => u.UserRole).FirstOrDefault(s => s.Id == id);
        }

        public User GetUserByUsername(String username)
        {
            return users.FirstOrDefault(s => s.Name == username);
        }

        public User GetUserByUsernameAndActive(String username)
        {
            return users.FirstOrDefault(s => (s.Name == username&&s.IsActive==true));
        }

        public User GetUserByEmail(String email)
        {
            return users.FirstOrDefault(s => s.Email == email);
        }

        public List<User> GetUsersByUsername(string username, int userId)
        {
            return users.Where(s => s.Name == username)
                        .Where(s => s.Id != userId)
                        .ToList();
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

        public void AddUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("Add a null user");
            }
            users.Add(user);
            context.SaveChanges();
        }

        public void UpdateUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("Update a null user");
            }
            context.SaveChanges();
        }

        public void DeleteUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("Delete null user");
            }
            users.Remove(user);
            context.SaveChanges();
        }

    }
}
