using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Learning.API
{
    public class ResponseFormat
    {
        public bool Result { get; set; }
        public string Message { get; set; }
        public object Description { get; set; }
    }
}
