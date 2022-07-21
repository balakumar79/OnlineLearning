using System;
using System.Collections.Generic;
using System.Text;

namespace Learning.Utils.Config
{
   public class AppConfig
    {
        public AppConfig()
        {

        }
        public AppConfig(SMTPConfig sMTPConfig,SecretKey secretKey,EncryptionKey encryptionKey)
        {
            SMTPConfig = sMTPConfig;
            SecretKey = secretKey;
            EncryptionKey = encryptionKey;
        }
        public SMTPConfig SMTPConfig{ get; }
        public SecretKey SecretKey { get; }
        public EncryptionKey EncryptionKey { get; set; }
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
        public string StudentSaltKey { get; set; }
    }
    public class EncryptionKey {
        public string Key { get; set; }
    }
}
