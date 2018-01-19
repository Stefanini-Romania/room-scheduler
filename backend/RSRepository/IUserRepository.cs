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
        User FindUserByCredential(string email, string password);
        User GetUserById(int id);
        User GetUserByEmail(String email);
        User GetUserByEmailAndActive(String email);
        User GetUserByResetPassCode(String ResetPass);
        List<User> GetUserByisActiv();
        List<User> GetUserByisInactiv();
        List<User> GetUsersByEmail(string email, int userId);
        void AddUser(User user);
        void UpdateUser(User user);
        void DeleteUser(User user);
        void SaveChanges();
    }
}
