using Learning.Utils.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Learning.Utils
{
   public class EmailService:IEmailService
    {
        private readonly AppConfig appConfig;
        public EmailService(AppConfig config)
        {
            appConfig = config;

        }
        public async Task SendEmailConfirmation(string emailId,string body)
        {
            await SendEmailAsync(emailId, "Email confirmation", body);
        }
        public async Task SendForgotPassword(string email, string body)
        {
            await SendEmailAsync(email, "Rest pasword link", body);
        }

        public async Task SendStudentInvitationLink(string email,string body)
        {
            await SendEmailAsync(email, "You have received an student invitation from a teacher", body);
        }

        public async Task<Boolean> SendEmailAsync(string toEmail,string subject, string message, List<string> ccEmail=default(List<string>),
            Stream fileAttachment=null, string filename=null)
        {
            try
            {
                using (var mailMsg=new MailMessage())
                {
                    mailMsg.From = new MailAddress(appConfig.SMTPConfig.FromEmail);
                    toEmail.Split(',').ToList().ForEach(emailaddress =>
                    {
                        mailMsg.To.Add(emailaddress);
                    });
                    if (ccEmail != null)
                    {
                        foreach (var item in ccEmail)
                        {
                            mailMsg.CC.Add(item);

                        }
                    }
                    mailMsg.Body = message;
                    mailMsg.Subject = subject;
                    if (fileAttachment != null) {
                        Attachment attachment = new Attachment(fileAttachment, filename);
                        mailMsg.Attachments.Add(attachment);
                    }
                    using (var client=new SmtpClient(appConfig.SMTPConfig.ServerName,appConfig.SMTPConfig.ServerPort))
                    {
                        if (appConfig.SMTPConfig.UserName == string.Empty || appConfig.SMTPConfig.Password == string.Empty)
                        {
                            client.UseDefaultCredentials = false;
                        }
                        else
                        {
                            client.UseDefaultCredentials = true;
                            client.Credentials = new System.Net.NetworkCredential(appConfig.SMTPConfig.UserName, appConfig.SMTPConfig.Password);
                        }
                        client.EnableSsl = appConfig.SMTPConfig.EnableSsl ?? false;
                        mailMsg.IsBodyHtml = true;
                        await client.SendMailAsync(mailMsg);
                    }
                }
                return true;
            }catch(Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }
        public async Task<string> GetEmailTemplateContent(string templateName)
        {
            var builder = new StringBuilder();
            using (var reader = File.OpenText($"wwwroot/emailtemp/{templateName}"))
            {
                builder.Append(await reader.ReadToEndAsync());
            }
            return builder.ToString();
        }
    }
}
