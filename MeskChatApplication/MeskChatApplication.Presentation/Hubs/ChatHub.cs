using System.Collections.Concurrent;
using System.Security.Claims;
using MESK.MediatR;
using MeskChatApplication.Application.Features.Commands.Messages.MarkAsRead;
using MeskChatApplication.Application.Features.Commands.Messages.SendMessage;
using MeskChatApplication.Application.Features.Commands.User.UpdateUserStatus;
using MeskChatApplication.Domain.Enums;
using MeskChatApplication.Presentation.Extensions;
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
            if (response.IsSuccess)
            {
                var user = response.Data;
                if (user is not null)
                {
                    await Clients.Others.SendAsync("UserStatusChanged", user);
                }
            }
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
                if (response.IsSuccess)
                {
                    var user = response.Data;
                    if (user is not null)
                    {
                        await Clients.Others.SendAsync("UserStatusChanged", user);
                    }
                }
            }
        }
        await base.OnDisconnectedAsync(exception);
    }

    public async Task SendMessageAsync(SendMessageDto request)
    {
        var senderGuid = GetCurrentUserId();
        var message = await _sender.Send(new SendMessageCommand(senderGuid, request.ReceiverId, request.Message));
        if (TryGetActiveConnections(request.ReceiverId, out var connections))
        {
            await Clients.Clients(connections).SendAsync("ReceiveMessage", message);
        }
        await Clients.Caller.SendAsync("SentMessage", message);
    }

    public async Task UpdateUserStatusAsync(UpdateUserStatusDto request)
    {
        var senderGuid = GetCurrentUserId();
        var response = await _sender.Send(new UpdateUserStatusCommand(senderGuid, request.Status));
        if (response.IsSuccess)
        {
            var user = response.Data;
            if (user is not null)
            {
                await Clients.Others.SendAsync("UserStatusChanged", user);
            }
        }
    }

    // TODO
    public async Task MarkAsReadAsync(MarkAsReadDto request, CancellationToken cancellationToken = default)
    {
        var senderGuid = GetCurrentUserId();
        var message = await _sender.Send(new MarkAsReadCommand(request.MessageId, senderGuid), cancellationToken);
        await Clients.Caller.SendAsync("MessageMarkedAsRead", message, cancellationToken);
        await Clients.User(senderGuid.ToString()).SendAsync("MessageReadByReceiver", message, cancellationToken);
    }

    # region Video Call Communication
    public async Task SendVideoCallOfferAsync(string receiverId, string offer)
        => await VideoCallCommunicationSenderAsync(receiverId, "ReceiveVideoCallOffer", offer);

    public async Task SendAnswerVideoCallOfferAsync(string receiverId, string answer)
        => await VideoCallCommunicationSenderAsync(receiverId, "ReceiveVideoCallAnswer", answer);

    public async Task SendIceCandidateVideoCallAsync(string receiverId, string candidate)
        => await VideoCallCommunicationSenderAsync(receiverId, "ReceiveIceCandidate", candidate);

    private async Task VideoCallCommunicationSenderAsync(string receiverId, string method, string message)
    {
        if (Guid.TryParse(receiverId, out var receiverGuid))
        {
            if (TryGetActiveConnections(receiverGuid, out var connections))
            {
                var senderId = GetCurrentUserId();
                await Clients.Clients(connections).SendAsync(method, senderId.ToString(), message);
            }
        }
    }
    #endregion

    # region Helpers Private Methods
    private static bool TryGetActiveConnections(Guid receiverId, out HashSet<string> connections)
    {
        connections = [];
        
        if (UserConnections.TryGetValue(receiverId, out var list) && list.Count > 0)
        {
            connections = list;
            return true;
        }

        return false;
    }

    private Guid GetCurrentUserId()
    {
        if(Context.User is null) throw new UnauthorizedAccessException();
        return Context.User.GetNameIdentifier();
    }
    #endregion
}