using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Brook.Totp.WebLogin2FA
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                InitDatabase(services);
            }


            host.Run();
        }

        /// <summary>
        /// 初始数据库
        /// </summary>
        /// <param name="service"></param>
        private static void InitDatabase(IServiceProvider service)
        {
            using (var scope = service.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<BrookTotpDBContext>();

                if (context.Users.Any())
                {
                    return;
                }

                var users = new List<Models.User> {
                        new Models.User {ID=1, Email="gaozengzhi@gmail.com" ,PassWord="123456",IsOpen2FA=false,SecretKeyFor2FA="secret+Brook" },
                        new Models.User {ID=2, Email="32289333@qq.com" ,PassWord="123456",IsOpen2FA=false,SecretKeyFor2FA="secret+32289333" }
                    };

                context.AddRange(users);

                context.SaveChanges();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
