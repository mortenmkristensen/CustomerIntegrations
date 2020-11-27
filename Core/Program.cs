using Database;
using System;
using Microsoft.Extensions.DependencyInjection;

namespace Core {
    class Program {
        public static readonly IServiceProvider Container = new ContainerBuilder().Build();
        static void Main(string[] args) {
            //IDBAccess dbAccess = new DBAccess(new DBConfig());
            //Stager stager = new Stager(dbAccess);
            //ScriptRunner scriptRunner = new ScriptRunner();
            //string ids = Environment.GetEnvironmentVariable("MP_IDS");//set with env variables
            //App app = new App(stager, scriptRunner, ids);
            var app = Container.GetService<IApp>();
            string interpreterPath = Environment.GetEnvironmentVariable("MP_INTERPRETERPATH"); //set with env virables
            app.Run(interpreterPath);
        }
    }
}
