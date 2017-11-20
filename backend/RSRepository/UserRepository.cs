using Microsoft.EntityFrameworkCore;
using RSData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;

namespace RSRepository
{
    public class UserRepository : IUserRepository
    {
        private RoomPlannerDevContext context;
        private DbSet<User> users;

        public UserRepository(RoomPlannerDevContext context)
        {
            this.context = context;
            users = context.Set<User>();
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }

        public IEnumerable<User> GetUsers()
        {
            return users.AsEnumerable();
        }

        public User FindUserByCredential(string username, string password)
        {
            // !! Nu trebuie sa-i aducem pe toti cu GetUsers. Trebuie cautat direct in DbSet-ul de users  !!

            var sha1 = System.Security.Cryptography.SHA1.Create();

            var hash = sha1.ComputeHash(Encoding.ASCII.GetBytes(password));
            var encryptedPass = Encoding.ASCII.GetString(hash);
            var returnvar = this.GetUsers().SingleOrDefault(c => c.Name == username && c.Password == encryptedPass);
            return returnvar;
        }

        public User GetUserById(long id)
        {
            return users.SingleOrDefault(s => s.Id == id);
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
