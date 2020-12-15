using Docker.DotNet;
using Docker.DotNet.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace Runner {
    public class DockerService : IDockerService {
        private DockerClient _client = new DockerClientConfiguration().CreateClient();
        public async Task PullImage() {
            await _client.Images
                .CreateImageAsync(new ImagesCreateParameters {
                    FromImage = "mmkristensen/ucngrp11",
                    Tag = "idle"
                },
                    new AuthConfig(),
                    new Progress<JSONMessage>());
        }
        public async Task<string> StartContainer(string connectionString, string collection, string database, string queuename, string interpreterpath,
                                        string messageBroker, string queueUser, string queuePassword, string consumerQueue) {
            var response = await _client.Containers.CreateContainerAsync(new CreateContainerParameters {
                Image = "mmkristensen/ucngrp11:idle",
                Name = queuename,
                Env = new List<string>() { $"MP_CONNECTIONSTRING={connectionString}", $"MP_COLLECTION={collection}", $"MP_DATABASE={database}",
                                            $"MP_QUEUENAME={queuename}", $"MP_INTERPRETERPATH={interpreterpath}", $"MP_MESSAGEBROKER={messageBroker}",
                                            $"MP_QUEUEUSER={queueUser}", $"MP_QUEUEPASSWORD={queuePassword}", $"MP_CONSUMERQUEUE={consumerQueue}"}
            });
            await _client.Containers.StartContainerAsync(response.ID, null);
            return queuename;
        }

        public async Task<IList<ContainerListResponse>> GetContainers() {
            return await _client.Containers.ListContainersAsync(new ContainersListParameters() { All = true });
        }

        public async Task PruneContainers() {
            try {
                await _client.Containers.PruneContainersAsync();
            }
            catch (Exception) {
                //TODO log exceptions.
            }
            
        }
    }
}
