using System;
using System.Collections.Generic;
using System.Text;

namespace Learning.Utils.Config
{
   public class AppConfig
    {
        public AppConfig(SMTPConfig sMTPConfig,SecretKey secretKey)
        {
            SMTPConfig = sMTPConfig;
            SecretKey = secretKey;
        }
        public SMTPConfig SMTPConfig{ get; }
        public SecretKey SecretKey { get; set; }
    }
    public class SMTPConfig
    {
        public string ServerName { get; set; }
        public int ServerPort { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool? EnableSsl { get; set; }
        public string FromEmail { get; set; }
    }
    public class SecretKey
    {
        public string  SecretKeyValue { get; set; }
    }
}
