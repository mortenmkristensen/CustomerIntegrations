using System.Threading.Tasks;

namespace Runner {
    public interface IApp {
        Task Start(string queueName);
    }
}
