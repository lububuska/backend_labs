namespace Consumer.Config;

public class RabbitMqSettings
{
    public string HostName { get; set; }
    public int Port { get; set; }
    public string OrderCreatedQueue { get; set; }
    public string UserName { get; set; } = "guest";
    public string Password { get; set; } = "guest";
}