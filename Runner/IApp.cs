using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace Runner {
    public interface IApp {
        Task Start(string queueName);
    }
}
