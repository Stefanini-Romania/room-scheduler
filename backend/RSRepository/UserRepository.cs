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
            return users.SingleOrDefault(c => c.Name == username && c.Password == password);
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
