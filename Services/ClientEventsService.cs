using Lib.AspNetCore.ServerSentEvents;
using Wikirace.Utils;

namespace Wikirace.Services;

/// <summary>
/// Service responsible for handling client events and sending server-sent events to clients.
/// </summary>
public class ClientEventsService {
    private readonly ILogger<ClientEventsService> _logger;
    private readonly IServerSentEventsService _serverSentEventsService;

    public ClientEventsService(ILogger<ClientEventsService> logger, IServerSentEventsService serverSentEventsService) {
        _logger = logger;
        _serverSentEventsService = serverSentEventsService;

        _serverSentEventsService.ClientConnected += OnClientConnected;
    }

    private void OnClientConnected(object? sender, ServerSentEventsClientConnectedArgs context) {
        _logger.LogInformation("Client connected: {UserId}", context.Client.User.GetUserId());

        var gameId = context.Request.HttpContext.GetRouteValue("gameId")?.ToString();
        if (gameId is not null) {
            _serverSentEventsService.AddToGroup(gameId, context.Client);
        }
    }

    public async Task SendEvent(string type, string gameId, string? playerId = null) {
        var clients = _serverSentEventsService.GetClients(gameId);

        var message = new ServerSentEvent {
            Type = type,
            Id = Guid.NewGuid().ToString(),
            Data = ["null"] // This is needed otherwise the client will not receive the event.
        };

        if (playerId is not null) {
            var client = clients.FirstOrDefault(c => c.User.GetUserId() == playerId);
            if (client is not null) {
                await client.SendEventAsync(message);
            }
        } else {
            foreach (var client in clients) {
                await client.SendEventAsync(message);
            }
        }
    }
}
