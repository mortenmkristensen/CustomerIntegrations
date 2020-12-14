using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Runner {
    class Program {
        public static readonly IServiceProvider Container = new ContainerBuilder().Build();
        static async Task Main(string[] args) {
            var app = Container.GetService<IApp>();
            await app.start("Scheduling_Queue");
        }
    }
}
