namespace MeskChatApplication.Application.Features.Commands.Authentication.Login;

public class LoginCommandResponse(string accessToken, string refreshToken)
{
    public string AccessToken { get; init; } =  accessToken;
    public string RefreshToken { get; init; } =  refreshToken;
}