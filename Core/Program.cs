using Database;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;

namespace Core {
    class Program {
        public static readonly IServiceProvider Container = new ContainerBuilder().Build();
        static void Main(string[] args) {
            var app = Container.GetService<IApp>();
            string interpreterPath = Environment.GetEnvironmentVariable("MP_INTERPRETERPATH"); //set with env virables
            int i = 0;
            while (true) {
                i = app.Run(interpreterPath, i);
                if(i > 30) {
                    return;
                }
                Thread.Sleep(1000);
            }
        }
    }
}
