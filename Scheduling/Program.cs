using System;

namespace Scheduling {
    class Program {
        static void Main(string[] args) {
            DockerService ds = new DockerService();
            ds.BuildImage();
        }
    }
}
