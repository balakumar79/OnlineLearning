using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Learning.Student.ViewModel
{
   public class SendEmailRequestModel
    {
        public string toEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<string> CC { get; set; }
        public IFormFile FormFile { get; set; }
        public string FileName { get; set; }
    }
}
