using Docker.DotNet.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Runner {
    public interface IDockerService {
        Task PullImage();
        Task<string> StartContainer(string connectionString, string collection, string database, string queuename, string interpreterpath,
                                        string messageBroker, string queueUser, string queuePassword, string consumerQueue, string logCollection);
        Task<IList<ContainerListResponse>> GetContainers();
        Task PruneContainers();
    }
}
