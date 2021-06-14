using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Learning.API
{
    public static class ResultFormatter
    {
        public static object JsonResponse(bool status, object result = null, string message = null)
        {
            return JsonResponse(status, result, message);
        }
    }
}
