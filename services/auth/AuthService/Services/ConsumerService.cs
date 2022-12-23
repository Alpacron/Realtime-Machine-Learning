using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using Newtonsoft.Json;
using System.Text;
using AuthService.Models;

namespace AuthService.Services;

public interface IConsumerService
{
}

public class ConsumerService : IConsumerService
{
    private readonly string AUTH_EXCHANGE = "auth";
    private readonly IMessagingService _messagingService;
    private readonly IServiceScopeFactory _scopeFactory;

    public ConsumerService(IMessagingService messagingService, IServiceScopeFactory serviceProvider)
    {
        _messagingService = messagingService;
        _scopeFactory = serviceProvider;

        Subscribe();
    }

    private void Subscribe()
    {
        _messagingService.Subscribe(AUTH_EXCHANGE, (BasicDeliverEventArgs ea, string queue, string request) => RouteCallback(ea, request), ExchangeType.Fanout, "");
    }

    private void RouteCallback(BasicDeliverEventArgs ea, string request)
    {
        using var scope = _scopeFactory.CreateScope();
        var das = scope.ServiceProvider.GetService<IDataAccessService>();
        if (das is not IDataAccessService dataAccessService)
            return;

        string data = Encoding.UTF8.GetString(ea.Body.ToArray());

        switch (request)
        {
            case "deleteuser":
                {
                    int id = int.Parse(data);

                    dataAccessService.DeleteCachedUser(id);
                    break;
                }
            case "updateuser":
                {
                    var updateduser = JsonConvert.DeserializeObject<User>(data);
                    if (updateduser is null)
                        break;

                    dataAccessService.UpdateCachedUser(updateduser);
                    break;
                }
            default:
                Console.WriteLine($"Request {request} Not Found");
                break;
        }
    }
}
