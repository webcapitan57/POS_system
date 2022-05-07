using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using POC.BL.Domain.user;
using UI_MVC;
using UI_MVC.Gcloud;
using UI_MVC.Services;

namespace POC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                try
                {
                    var userManager = serviceProvider.GetRequiredService<UserManager<UserAccount>>();

                    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                    var storage = serviceProvider.GetRequiredService<GcloudStorage>();
                    var webhostenv = serviceProvider.GetRequiredService<IWebHostEnvironment>();

                    IdentityInitializer.SeedData
                        (userManager, roleManager,storage,webhostenv);
                }
                catch(Exception e)
                {
                    var ex = e;
                }

                host.Run();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}