using RSData.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RSRepository
{
    public interface IUserRepository
    {
        IEnumerable<User> GetUsers();
        User GetUserById(long id);
        void AddUser(User user);
        void UpdateUser(User user);
        void DeleteUser(User user);
        void SaveChanges();
    }
}
