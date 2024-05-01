namespace Argo.CA.Infrastructure.Messaging;

public class RabbitMqOptions
{
    public string HostName { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public ushort Port { get; set; } = 5672; // default RMQ port
    public string VirtualHost { get; set; } = "/";
}