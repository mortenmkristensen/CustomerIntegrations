using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace Scheduling {
    class Program {
        static async Task Main(string[] args) {
            await CreateHostBuilder(args).Build().RunAsync();

            static IHostBuilder CreateHostBuilder(string[] args) =>
                Host.CreateDefaultBuilder(args)
                    .ConfigureServices((_, services) =>
                        services.AddHostedService<Scheduler>());
        }
        
    }
}
