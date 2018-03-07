using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSService.BusinessLogic
{
    public interface IUserService
    {
        bool IsUniqueEmail(String email);

        bool IsUniqueEmailEdit(String email, int userId);

        bool IsValidRole(List<int> userRole);

        bool IsActiveUser(String email);

        bool GoodEmailFormat(string email);

        bool WeakPassword(string pass);
    }
}
