using RSData.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RSRepository
{
    public interface IUserRepository
    {
        List<User> GetUsers2();
        List<User> GetUsers();
        User FindUserByCredential(string username, string password);
        User GetUserById(long id);
        User GetUserByUsername(string username);
        User GetUserByEmail(string email);
        void AddUser(User user);
        void UpdateUser(User user);
        void DeleteUser(User user);
        void SaveChanges();
    }
}
