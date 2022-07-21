using Learning.Entities;
using Learning.Utils.Config;
using Learning.ViewModel.Account;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Learning.Utils
{
    public class CommonData : BackgroundService
    {
        private readonly ILogger<CommonData> _logger;
        private static AppConfig _appConfig;
        private static EncryptionKey _encryptionKey;

        public CommonData(ILogger<CommonData> logger,AppConfig appConfig,EncryptionKey encryptionKey)
        {
            _logger = logger;
            _appConfig = appConfig;
            _encryptionKey = encryptionKey;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }

        public static void SeedRoles(Microsoft.AspNetCore.Identity.RoleManager<AppRole> roleManager)
        {
            foreach (var role in Enum.GetValues(typeof(Learning.Utils.Enums.Roles)))
            {
                if (!roleManager.RoleExistsAsync(role.ToString()).Result)
                {

                    AppRole appRole = new AppRole();
                    appRole.Name = role.ToString();
                    Microsoft.AspNetCore.Identity.IdentityResult result = roleManager.CreateAsync(appRole).Result;
                }
            }
        }
        public static string EncryptString(string plainText, string key)
        {
            byte[] iv = new byte[16];
            byte[] array;
            EncryptionKey encryption = _encryptionKey;
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(array);
        }

        public static string DecryptString(string cipherText, string key)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(cipherText);
            var appconfig = _appConfig;
            if (string.IsNullOrEmpty(key))
                key = new AppConfig().EncryptionKey.Key;
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}


