namespace Core {
    public interface IApp {
        int Run(string interpreterPath, int count, string queueName, string consumerQuequeName);
    }
}