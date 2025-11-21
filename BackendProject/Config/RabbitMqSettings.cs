namespace BackendProject.Config;

public class RabbitMqSettings
{
    public string HostName { get; set; } = default!;
    public int Port { get; set; }
    public string OrderCreatedQueue { get; set; }
}
