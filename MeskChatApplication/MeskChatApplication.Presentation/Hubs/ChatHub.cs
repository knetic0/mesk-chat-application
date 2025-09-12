using System.Collections.Concurrent;
using System.Security.Claims;
using MESK.MediatR;
using MeskChatApplication.Application.Exceptions;
using MeskChatApplication.Application.Features.Commands.Messages.MarkAsRead;
using MeskChatApplication.Application.Features.Commands.Messages.SendMessage;
using MeskChatApplication.Application.Features.Commands.User.UpdateUserStatus;
using MeskChatApplication.Application.Services;
using MeskChatApplication.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using UnauthorizedAccessException = MeskChatApplication.Application.Exceptions.UnauthorizedAccessException;

namespace MeskChatApplication.Presentation.Hubs;

[Authorize]
public sealed class ChatHub(ISender sender, IUnitOfWork unitOfWork) : Hub
{
    private readonly ISender _sender = sender;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    private static readonly ConcurrentDictionary<Guid, HashSet<string>> UserConnections = new();

    public override async Task OnConnectedAsync()
    {
        var userGuid = GetCurrentUserId();
        var connectionId = Context.ConnectionId;
        UserConnections.AddOrUpdate(userGuid,
            new HashSet<string> { connectionId },
            (_, connections) =>
            {
                connections.Add(connectionId);
                return connections;
            });
        if (UserConnections[userGuid].Count == 1)
        {
            await _sender.Send(new UpdateUserStatusCommand(userGuid, Status.Online));
            await _unitOfWork.SaveChangesAsync();
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
                await _sender.Send(new UpdateUserStatusCommand(userGuid, Status.Offline));
                await _unitOfWork.SaveChangesAsync();
            }
        }
        await base.OnDisconnectedAsync(exception);
    }

    public async Task SendMessageAsync(SendMessageDto request, CancellationToken cancellationToken = default)
    {
        var senderGuid = GetCurrentUserId();
        var message = await _sender.Send(new SendMessageCommand(senderGuid, request.ReceiverId, request.Message), cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        if (!UserConnections.TryGetValue(request.ReceiverId, out var connections)) throw new UnauthorizedAccessException();
        await Clients.Clients(connections).SendAsync("ReceiveMessage", message, cancellationToken);
        await Clients.Caller.SendAsync("SentMessage", message, cancellationToken);
    }

    public async Task MarkAsReadAsync(MarkAsReadDto request, CancellationToken cancellationToken = default)
    {
        var senderGuid = GetCurrentUserId();
        var message = await _sender.Send(new MarkAsReadCommand(request.MessageId, senderGuid), cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
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