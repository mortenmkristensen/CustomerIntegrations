using Database;
using System;
using Microsoft.Extensions.DependencyInjection;

namespace Core {
    class Program {
        public static readonly IServiceProvider Container = new ContainerBuilder().Build();
        static void Main(string[] args) {
            var app = Container.GetService<IApp>();
            string interpreterPath = Environment.GetEnvironmentVariable("MP_INTERPRETERPATH"); //set with env virables
            app.Run(interpreterPath);
        }
    }
}
