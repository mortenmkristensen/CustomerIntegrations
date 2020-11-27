using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Database;

namespace Core {
    class ContainerBuilder {
        public IServiceProvider Build() {
            var container = new ServiceCollection();
            container.AddSingleton<IDBConfig, DBConfig>();
            container.AddSingleton<IDBAccess, DBAccess>();
            container.AddSingleton<IApp, App>();
            container.AddTransient<IStager, Stager>();
            container.AddTransient<IScriptRunner, ScriptRunner>();
            return container.BuildServiceProvider();
        }
    }
}
