using Docker.DotNet.Models;
using Docker.DotNet;

namespace DaiManagementUi
{
    public class Docker
    {
        public static List<ContainerListResponse> GetAllContainers()
        {
            var client = GetClient();
            var parameters = new ContainersListParameters
            {
                All = true
            };

            return client.Containers.ListContainersAsync(parameters).Result.ToList();
        }

        public static void StartContainer(string id)
        {
            if (IsRunning(id))
            {
                return;
            }

            var client = GetClient();
            var parameters = new ContainerStartParameters();
            client.Containers.StartContainerAsync(id, parameters).Wait();
        }

        public static void StopContainer(string id)
        {
            if (!IsRunning(id))
            {
                return;
            }

            var client = GetClient();
            var parameters = new ContainerStopParameters();
            client.Containers.StopContainerAsync(id, parameters).Wait();
        }

        public static bool IsRunning(string id)
        {
            var container = GetAllContainers().FirstOrDefault(c => c.ID == id);
            if (container != null)
            {
                return container.State == "running";
            }

            return false;
        }

        public static void CreateContainer(string tag)
        {
            var client = GetClient();
            var parameters = new ImagesCreateParameters
            {
                FromImage = tag
            };
            client.Images.CreateImageAsync(parameters, null, new Progress<JSONMessage>()).Wait();
            client.Containers.CreateContainerAsync(new CreateContainerParameters()
            {
                Image = tag
            }).Wait();
        }

        public static void DeleteContainer(string id)
        {
            var client = GetClient();
            var parameters = new ContainerRemoveParameters 
            { 
                Force = true 
            };
            client.Containers.RemoveContainerAsync(id, parameters).Wait();
        }

        private static DockerClient GetClient()
            => new DockerClientConfiguration(new Uri("unix:///var/run/docker.sock")).CreateClient();
    }
}
