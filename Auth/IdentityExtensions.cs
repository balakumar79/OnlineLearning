using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
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
        public static int GetTutorId(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst(CustomClaimTypes.TutorID);
            int.TryParse(claim?.Value, out int tid);
            return tid;
        }

        /// <summary>
        /// Returns student id
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        public static string GetStudentId(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst(CustomClaimTypes.StudentId);
            return (claim != null) ? claim.Value : "0";
        }
        public static List<int> GetChildIds(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst(CustomClaimTypes.ChildIds);
            return (claim != null) ? claim.Value.Split(",").ToList().ConvertAll(int.Parse) : new List<int>();
        }
        public static int GetTeacherId(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst(CustomClaimTypes.TeacherId);
            return (claim != null) ? Convert.ToInt32(claim.Value) : 0;
        }

    }

}
