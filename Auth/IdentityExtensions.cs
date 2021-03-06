using Learning.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using static Learning.ViewModel.Account.AuthorizationModel;

namespace Learning.Auth
{
   public static class IdentityExtensions
    {
     public static string GetUserID(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst(ClaimTypes.NameIdentifier);
            return (claim != null) ? claim.Value : string.Empty;
        }
        public static string GetTutorId(this IIdentity identity)
        {                                             
            var claim = ((ClaimsIdentity)identity).FindFirst(CustomClaimTypes.TutorID);
            return (claim != null) ? claim.Value : string.Empty;
        }

        /// <summary>
        /// Returns student id
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        public static string GetStudentId(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst(CustomClaimTypes.StudentId);
            return (claim != null) ? claim.Value : string.Empty;
        }
        public static List<int> GetChildIds(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst(CustomClaimTypes.ChildIds);
            return (claim != null) ? claim.Value.Split(",").ToList().ConvertAll(int.Parse) : new List<int>();
        }

    }
   
}
