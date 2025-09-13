using System.Collections.Concurrent;
using System.Security.Claims;
using MESK.MediatR;
using MeskChatApplication.Application.Features.Commands.Messages.MarkAsRead;
using MeskChatApplication.Application.Features.Commands.Messages.SendMessage;
using MeskChatApplication.Application.Features.Commands.User.UpdateUserStatus;
using MeskChatApplication.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using UnauthorizedAccessException = MeskChatApplication.Application.Exceptions.UnauthorizedAccessException;

namespace MeskChatApplication.Presentation.Hubs;

[Authorize]
public sealed class ChatHub(ISender sender) : Hub
{
    private readonly ISender _sender = sender;

    private static readonly ConcurrentDictionary<Guid, HashSet<string>> UserConnections = new();

    public override async Task OnConnectedAsync()
    {
        var userGuid = GetCurrentUserId();
        var connectionId = Context.ConnectionId;
        UserConnections.AddOrUpdate(userGuid,
            [connectionId],
            (_, connections) =>
            {
                connections.Add(connectionId);
                return connections;
            });
        if (UserConnections.TryGetValue(userGuid, out var userConnections) && userConnections.Count == 1)
        {
            var response = await _sender.Send(new UpdateUserStatusCommand(userGuid, Status.Online));
            if(response.IsSuccess) await Clients.All.SendAsync("UserStatusChanged", userGuid, Status.Online);
        }
        await base.OnConnectedAsync();
    }
    
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userGuid = GetCurrentUserId();
        var connectionId = Context.ConnectionId;
        if (UserConnections.TryGetValue(userGuid, out var connections))
        {
            connections.Remove(connectionId);
            if (connections.Count == 0)
            {
                UserConnections.TryRemove(userGuid, out _);
                var response = await _sender.Send(new UpdateUserStatusCommand(userGuid, Status.Offline));
                if(response.IsSuccess) await Clients.All.SendAsync("UserStatusChanged", userGuid, Status.Offline);
            }
        }
        await base.OnDisconnectedAsync(exception);
    }

    public async Task SendMessageAsync(SendMessageDto request)
    {
        var senderGuid = GetCurrentUserId();
        var message = await _sender.Send(new SendMessageCommand(senderGuid, request.ReceiverId, request.Message));
        if (UserConnections.TryGetValue(request.ReceiverId, out var connections) && connections.Count > 0)
        {
            await Clients.Clients(connections).SendAsync("ReceiveMessage", message);
        }
        await Clients.Caller.SendAsync("SentMessage", message);
    }

    public async Task MarkAsReadAsync(MarkAsReadDto request, CancellationToken cancellationToken = default)
    {
        var senderGuid = GetCurrentUserId();
        var message = await _sender.Send(new MarkAsReadCommand(request.MessageId, senderGuid), cancellationToken);
        await Clients.Caller.SendAsync("MessageMarkedAsRead", message, cancellationToken);
        await Clients.User(message.SenderId.ToString()).SendAsync("MessageReadByReceiver", message, cancellationToken);
    }

    private Guid GetCurrentUserId()
    {
        var senderId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(senderId, out var senderGuid)) throw new UnauthorizedAccessException();
        return senderGuid;
    }
}