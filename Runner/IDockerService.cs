using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Runner {
    public interface IDockerService {
        Task PullImage();
        Task StartContainer(string connectionString, string collection, string database, string queuename, string interpreterpath,
                                        string messageBroker, string queueUser, string queuePassword, string consumerQueue);
    }
}
