﻿
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace RSService.Validators
//{
//    public class CustomAuthorizationFilter : IDashboardAuthorizationFilter
//    {
//        public bool Authorize([NotNull] DashboardContext context)
//        {
//            var httpcontext = context.GetHttpContext();
//            return httpcontext.User.Identity.IsAuthenticated;
//        }
//    }
//}