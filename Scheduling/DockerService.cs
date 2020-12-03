using System;
using System.Collections.Generic;
using System.Text;
using Docker.DotNet;
using Docker.DotNet.Models;
using System.IO;

namespace Scheduling {
    class DockerService {
        DockerClient client = new DockerClientConfiguration().CreateClient();
        public void BuildImage() {
            using (FileStream fs = File.OpenRead(@"C:\Datamatiker\Repos\CustomerIntegrations\CustomerIntegrations\Dockerfile")) {
                byte[] dockerfile = new byte[3072];
                fs.Read(dockerfile, 0, dockerfile.Length);
                ImageBuildParameters ibp = new ImageBuildParameters();
                ibp.BuildArgs.Add("buildtime_connectionstring", "mongodb://192.168.0.73:27017");
                //client.Images.BuildImageFromDockerfileAsync(fs, )
            } 
            
        }
    }
}
