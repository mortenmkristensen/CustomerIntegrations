using System;
using Microsoft.Extensions.DependencyInjection;

namespace Runner {
    class Program {
        public static readonly IServiceProvider Container = new ContainerBuilder().Build();
        static void Main(string[] args) {
            var app = Container.GetService<IApp>();
            app.ListenToQueue("testQueue");
        }
    }
}
