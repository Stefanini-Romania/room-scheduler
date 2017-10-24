using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RSService.Controllers
{
    
    public abstract class BaseController : Controller
    {
        private int _currentUser;

        public int GetCurrentUserId()
        {
            return 1;
        }

        public void SetCurrentUser(int user)
        {
            _currentUser = user;
        }

    }
}
