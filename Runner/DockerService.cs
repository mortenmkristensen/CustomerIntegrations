using Docker.DotNet;
using Docker.DotNet.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Runner {
    public class DockerService : IDockerService {
        private DockerClient _client = new DockerClientConfiguration().CreateClient();

        //This method uses the docker client to pull the specified image from DockerHub
        public async Task PullImage() {
            await _client.Images
                .CreateImageAsync(new ImagesCreateParameters {
                    FromImage = "mmkristensen/ucngrp11",
                    Tag = "latest"
                },
                    new AuthConfig(),
                    new Progress<JSONMessage>());
        }

        //This method creates a container based on the specied image(the one pulled from DockerHub) sets all the environment variables that are needed in the container
        //the container is then started
        public async Task<string> StartContainer(string connectionString, string collection, string database, string queuename, string interpreterpath,
                                        string messageBroker, string queueUser, string queuePassword, string consumerQueue) {
            //create container
            var response = await _client.Containers.CreateContainerAsync(new CreateContainerParameters {
                Image = "mmkristensen/ucngrp11:latest",
                Name = queuename,
                Env = new List<string>() { $"MP_CONNECTIONSTRING={connectionString}", $"MP_COLLECTION={collection}", $"MP_DATABASE={database}",
                                            $"MP_QUEUENAME={queuename}", $"MP_INTERPRETERPATH={interpreterpath}", $"MP_MESSAGEBROKER={messageBroker}",
                                            $"MP_QUEUEUSER={queueUser}", $"MP_QUEUEPASSWORD={queuePassword}", $"MP_CONSUMERQUEUE={consumerQueue}"}
            });
            //start container
            await _client.Containers.StartContainerAsync(response.ID, null);
            return queuename;
        }
        //This method gets a list of all the containers in the Docker environment
        public async Task<IList<ContainerListResponse>> GetContainers() {
            return await _client.Containers.ListContainersAsync(new ContainersListParameters() { All = true });
        }

        //This method removes all containers that are not running
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
