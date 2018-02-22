using RSRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSService.BusinessLogic
{
    public class UserService : IUserService
    {
        private IUserRepository _userRepository;
        private IRoleRepository _roleRepository;



        public UserService(IUserRepository userRepository, IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        public bool IsUniqueEmail(String email)
        {
            var user = _userRepository.GetUserByEmail(email);

            if (user == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsUniqueEmailEdit(String email, int userId)
        {
            var users = _userRepository.GetUsersByEmail(email, userId);

            if (users.Count() > 0)
            {
                return false;
            }
            return true;
        }

        public bool IsValidRole(List<int> userRole)
        {
            foreach (var roleId in userRole)
            {
                var role = _roleRepository.GetRoleById(roleId);

                if (role == null)
                {
                    return false;
                }
            }
            return true;
        }

        public bool IsActiveUser(String email)
        {
            var activeUser = _userRepository.GetUserByEmailAndActive(email);

            if (activeUser == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
