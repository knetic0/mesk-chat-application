using MeskChatApplication.Application.Features.Commands.Authentication.Login;

namespace MeskChatApplication.Application.Features.Commands.Authentication.RefreshToken;

public sealed class RefreshTokenCommandResponse(string accessToken, string refreshToken) 
    : LoginCommandResponse(accessToken, refreshToken) { }