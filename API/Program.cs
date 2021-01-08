using Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace API {
    public class Program {
        public static void Main(string[] args) {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => {
                    webBuilder.ConfigureServices((_, services) => {
                        services.AddSingleton<IDBAccess, DBAccess>();
                        services.AddSingleton<IDBConfig, DBConfig>();
                    });
                    webBuilder.UseStartup<Startup>()
                    .UseSerilog((context, config) => {
                        config.ReadFrom.Configuration(context.Configuration);
                    });
                });
    }
}
