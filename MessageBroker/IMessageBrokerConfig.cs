namespace MessageBroker {
    public interface IMessageBrokerConfig {
        string HostName { get; set; }
        string Password { get; set; }
        string UserName { get; set; }
    }
}