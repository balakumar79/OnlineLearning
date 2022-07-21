using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Learning.Utils
{
    public interface IEmailService
    {
        Task<Boolean> SendEmailAsync(string toEmail, string subject, string message, List<string> ccEmail = default(List<string>),
           Stream fileAttachment = null, string filename = null);
        Task<string> GetEmailTemplateContent(string templateName);
        Task SendEmailConfirmation(string emailId, string body);
        Task SendForgotPassword(string email, string body);
        Task SendStudentInvitationLink(string email, string body);
    }
}
