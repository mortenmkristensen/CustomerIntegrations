using System;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;

namespace Core {
    class Program {
        //Makes a new container for this project. 
        public static readonly IServiceProvider Container = new ContainerBuilder().Build();

        //This method gets a path from MP_INTERPRETERPATH. After the method Run is executed, it checks if count is over 30 (if the queue is empty count is counted up by 1), and if it's over 30, the program is closed. 
        static void Main(string[] args) {
            var app = Container.GetService<IApp>();
            string interpreterPath = Environment.GetEnvironmentVariable("MP_INTERPRETERPATH"); //set with env virables
            int i = 0;
            while (true) {
                //the container runs the Run method (happens every half second). 
                i = app.Run(interpreterPath, i, Environment.GetEnvironmentVariable("MP_QUEUENAME"), Environment.GetEnvironmentVariable("MP_CONSUMERQUEUE"));
                if(i > 30) {
                    return;
                }
                Thread.Sleep(500);
            }
        }
    }
}
