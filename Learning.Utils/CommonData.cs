using Learning.Entities;
using Learning.ViewModel.Account;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Learning.Utils
{
    public class CommonData : BackgroundService
    {
        private readonly ILogger<CommonData> _logger;
        public CommonData(ILogger<CommonData> logger)
        {
            _logger = logger;
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
            foreach (var role in Enum.GetValues(typeof(Learning.Utils.Enums.RoleEnum)))
            {
                if (!roleManager.RoleExistsAsync(role.ToString()).Result)
                {
                   
                    AppRole appRole = new AppRole();
                    appRole.Name = role.ToString();
                    Microsoft.AspNetCore.Identity.IdentityResult result = roleManager.CreateAsync(appRole).Result;
                }
            }
        }

       

    }
}
