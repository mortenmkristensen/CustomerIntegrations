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
                    Tag = "latest"
                },
                    new AuthConfig(),
                    new Progress<JSONMessage>());
        }
        public async Task StartContainer(string connectionString, string collection, string database, string queuename, string interpreterpath,
                                        string messageBroker, string queueUser, string queuePassword, string consumerQueue) {
            var containers = await _client.Containers.ListContainersAsync(new ContainersListParameters() { All = true });
            foreach (var container in containers) {
                if (container.Names != null && container.Names.Count > 0) {
                    string name = container.Names[0].Split("/")[1];
                    if (name == queuename) {
                        await _client.Containers.StartContainerAsync(container.ID, null);
                        return;
                    }
                }
            }
            var response = await _client.Containers.CreateContainerAsync(new CreateContainerParameters {
                Image = "mmkristensen/ucngrp11",
                Name = queuename,
                Env = new List<string>() { $"MP_CONNECTIONSTRING={connectionString}", $"MP_COLLECTION={collection}", $"MP_DATABASE={database}",
                                            $"MP_QUEUENAME={queuename}", $"MP_INTERPRETERPATH={interpreterpath}", $"MP_MESSAGEBROKER={messageBroker}",
                                            $"MP_QUEUEUSER={queueUser}", $"MP_QUEUEPASSWORD={queuePassword}", $"MP_CONSUMERQUEUE={consumerQueue}"}
            });
            await _client.Containers.StartContainerAsync(response.ID, null);
        }

        public async Task<IList<ContainerListResponse>> GetContainers() {
            return await _client.Containers.ListContainersAsync(new ContainersListParameters() { All = true });
        }

        public async Task RemoveContainer(string containerName) {
            var containers = await _client.Containers.ListContainersAsync(new ContainersListParameters() { All = true});
            foreach (var container in containers) {
                if (container.Names != null && container.Names.Count > 0) {
                    string name = container.Names[0].Split("/")[1];
                    if (name == containerName) {
                        await _client.Containers.RemoveContainerAsync(container.ID, null);
                    }
                }
            }
            
        }
    }
}
