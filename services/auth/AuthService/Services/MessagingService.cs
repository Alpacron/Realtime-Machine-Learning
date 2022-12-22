using k8s.Models;
using k8s;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace AuthDataAccessService.Services;

public interface IMessagingService
{
    void Subscribe(string exchange, Action<BasicDeliverEventArgs, string, string> callback, string exchangeType, string? bindingKey = null);
    void Publish(string exchange, string route, string request, string? queue = null, byte[]? message = null);
    Task<string> PublishAndRetrieve(string exchange, string request, byte[]? message = null);
    Task<string> RestCall(string method, string path, string? message = null);
}

public class MessagingService : IMessagingService
{
    private readonly int MESSAGE_TIMEOUT = 20000;

    private readonly IConnection _connection;
    private readonly string _serviceGuid;
    private readonly HttpClient _client;

    public MessagingService(IConfiguration configuration)
    {
        _client = new HttpClient();

        var factory = new ConnectionFactory()
        {
            HostName = configuration["RabbitMq:Connection:Host"],
            UserName = configuration["RabbitMq:Connection:Username"],
            Password = configuration["RabbitMq:Connection:Password"],
            Port = int.Parse(configuration["RabbitMq:Connection:Port"])
        };

        _connection = factory.CreateConnection();
        _serviceGuid = Guid.NewGuid().ToString();
    }

    public void Subscribe(string exchange, Action<BasicDeliverEventArgs, string, string> callback, string exchangeType, string? bindingKey = null)
    {
        string queue = $"{exchange}-{exchangeType}-consumer{(exchangeType == ExchangeType.Fanout ? $"-{_serviceGuid}" : "")}";
        IModel channel = CreateChannel(exchange, queue, exchangeType, bindingKey);

        CreateConsumer(channel, exchange, queue, callback, true);
    }

    public void Publish(string exchange, string route, string request, string? queue = null, byte[]? message = null)
    {
        IModel channel = CreateChannel(exchange, queue);
        Publish(channel, exchange, route, request, queue, message);
        channel.Close();
    }

    public async Task<string> PublishAndRetrieve(string exchange, string request, byte[]? message = null)
    {
        message ??= Encoding.UTF8.GetBytes("null");
        string requestGuid = Guid.NewGuid().ToString();
        string route = $"{requestGuid}.{_serviceGuid}.request";
        string queue = $"{exchange}-messaging-{_serviceGuid}";
        IModel channel = CreateChannel(exchange, queue, ExchangeType.Topic, $"*.{_serviceGuid}.response");
        string consumeTag = "";

        TaskCompletionSource<string> receivedMessage = new();

        var callback = (BasicDeliverEventArgs ea, string _queue, string _request) =>
        {
            if (ea.RoutingKey != $"{requestGuid}.{_serviceGuid}.response")
            {
                channel.BasicNack(ea.DeliveryTag, false, true);
                return;
            }

            channel.BasicAck(ea.DeliveryTag, false);

            receivedMessage.SetResult(Encoding.UTF8.GetString(ea.Body.ToArray()));

            channel.BasicCancel(consumeTag);
            channel.Close();
        };

        consumeTag = CreateConsumer(channel, exchange, queue, callback, false);

        Publish(channel, exchange, queue, route, request, message);

        bool didReceive = await Task.WhenAny(receivedMessage.Task, Task.Delay(MESSAGE_TIMEOUT)) == receivedMessage.Task;

        if (didReceive)
            return receivedMessage.Task.Result;

        channel.BasicCancel(consumeTag);
        channel.Close();
        throw new Exception($"[{DateTime.Now:HH:mm:ss}] request timed out request: {request}, on exchange: {exchange}");
    }

    public async Task<string> RestCall(string method, string path, string? message = null)
    {
        Console.WriteLine($"{method} {path}");
        var config = KubernetesClientConfiguration.BuildConfigFromConfigFile();
        var client = new Kubernetes(config);

        V1ServiceList services = client.ListNamespacedService("rml");
        Console.WriteLine(services);
        foreach (var s in services.Items)
        {
            Console.WriteLine(s.Spec.ClusterIP);
            try
            {
                Console.WriteLine(await _client.GetAsync($"{s.Spec.ClusterIP}{path}"));
            }
            catch
            {

            }
        }
        throw new NotImplementedException();
    }

    private static void Publish(IModel channel, string exchange, string route, string request, string? queue = null, byte[]? message = null)
    {
        message ??= Encoding.UTF8.GetBytes("null");
        Dictionary<string, object> headers = new()
        {
            { "request", request }
        };

        if (queue != null)
            headers.Add("queue", queue);

        IBasicProperties props = channel.CreateBasicProperties();
        props.Headers = headers;

        channel.BasicPublish(exchange: exchange, routingKey: route, basicProperties: props, body: message);
        Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Sent: {request}, on exchange: {exchange}, with content: {Encoding.UTF8.GetString(message)}");
    }

    private static string CreateConsumer(IModel channel, string exchange, string queue, Action<BasicDeliverEventArgs, string, string> callback, bool autoAck)
    {
        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
            string _queue = ea.BasicProperties.Headers is not null && ea.BasicProperties.Headers.ContainsKey("queue") ? Encoding.UTF8.GetString((byte[])ea.BasicProperties.Headers["queue"]) : "";
            string _request = ea.BasicProperties.Headers is not null && ea.BasicProperties.Headers.ContainsKey("request") ? Encoding.UTF8.GetString((byte[])ea.BasicProperties.Headers["request"]) : "";
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Received: {_request}, on exchange: {exchange}, with content: {Encoding.UTF8.GetString(ea.Body.ToArray())}");
            callback(ea, _queue, _request);
        };

        return channel.BasicConsume(queue: queue, autoAck: autoAck, consumer: consumer);
    }

    private IModel CreateChannel(string exchange, string? queue = null, string? exchangeType = null, string? bindingKey = null)
    {
        Dictionary<string, object> args = new()
        {
            { "x-expires", 3000 }
        };

        IModel channel = _connection.CreateModel();

        if(exchangeType is not null)
            channel.ExchangeDeclare(exchange: exchange, type: exchangeType);

        if (queue is not null)
            channel.QueueDeclare(queue: queue, exclusive: false, autoDelete: false, arguments: args);


        if (bindingKey is not null && queue is not null && exchangeType is not null)
            channel.QueueBind(queue: queue, exchange: exchange, routingKey: bindingKey);

        return channel;
    }
}
