using System.Security.Cryptography;
using Mapster;
using MeskChatApplication.Application.Exceptions;
using MeskChatApplication.Application.Features.Commands.Authentication.ForgotPassword;
using MeskChatApplication.Application.Features.Commands.Authentication.Login;
using MeskChatApplication.Application.Features.Commands.Authentication.Logout;
using MeskChatApplication.Application.Features.Commands.Authentication.RefreshToken;
using MeskChatApplication.Application.Features.Commands.Authentication.Register;
using MeskChatApplication.Application.Features.Commands.Authentication.ResetPassword;
using MeskChatApplication.Application.Features.Queries.Authentication.Me;
using MeskChatApplication.Application.Notifications.Email;
using MeskChatApplication.Application.Services;
using MeskChatApplication.Domain.Dtos;
using MeskChatApplication.Domain.Entities;
using UnauthorizedAccessException = MeskChatApplication.Application.Exceptions.UnauthorizedAccessException;

namespace MeskChatApplication.Persistance.Services;

public sealed class AuthenticationService(IJwtService jwtService, IUserService userService, 
    IRefreshTokenService refreshTokenService, IUnitOfWork unitOfWork, IPasswordResetTokenService passwordResetTokenService) : IAuthenticationService
{
    private readonly IUserService _userService =  userService;
    private readonly IJwtService _jwtService = jwtService;
    private readonly IRefreshTokenService _refreshTokenService = refreshTokenService;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IPasswordResetTokenService _passwordResetTokenService = passwordResetTokenService;
    
    public async Task<LoginCommandResponse> LoginAsync(LoginCommand command, CancellationToken cancellationToken = default)
    {
        var user = await _userService.GetAsync(user => user.Email == command.Email, cancellationToken);
        if (user is null) throw new NotFoundException(nameof(User), command.Email);
        var isPasswordValid = VerifyPassword(command.Password, user.PasswordHash, user.PasswordSalt);
        if (!isPasswordValid) throw new InvalidCredentialException();
        var accessToken = _jwtService.Generate(user);
        var refreshToken = await _refreshTokenService.CreateAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return new LoginCommandResponse(accessToken, refreshToken);
    }

    public async Task RegisterAsync(RegisterCommand command, CancellationToken cancellationToken = default)
    {
        var user = await _userService.GetAsync(user => user.Email == command.Email && user.Username == command.Username, cancellationToken);
        if (user is not null) throw new UserExistsException();
        HashPassword(command.Password, out var hashed, out var salted);
        var entity = new User(command.FirstName, command.LastName,command.Email, command.Username, hashed, salted);
        await _userService.CreateAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<ApplicationUser> GetCurrentUserAsync(GetCurrentUserQuery query,
        CancellationToken cancellationToken = default)
    {
        var user = await _userService.GetAsync(user => user.Id == query.UserId, cancellationToken);
        if (user is null) throw new NotFoundException(nameof(User), query.UserId);
        var dto = user.Adapt<ApplicationUser>();
        return dto;
    }

    public async Task<RefreshTokenCommandResponse> RefreshTokenAsync(RefreshTokenCommand command,
        CancellationToken cancellationToken = default)
    {
        var refreshToken = await _refreshTokenService.GetAsync(r => r.Token == command.RefreshToken, cancellationToken);
        if (refreshToken is null || !refreshToken.IsActive) throw new UnauthorizedAccessException();
        _refreshTokenService.MarkAsUsed(refreshToken, cancellationToken);
        var accessToken = _jwtService.Generate(refreshToken.User);
        var refreshTokenCreated = await _refreshTokenService.CreateAsync(refreshToken.User, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return new RefreshTokenCommandResponse(accessToken, refreshTokenCreated);
    }

    public async Task LogoutAsync(LogoutCommand command, CancellationToken cancellationToken = default)
    {
        var refreshToken = await _refreshTokenService.GetAsync(r => r.Token == command.RefreshToken, cancellationToken);
        if (refreshToken is null || !refreshToken.IsActive) throw new UnauthorizedAccessException();
        _refreshTokenService.MarkAsRevoked(refreshToken, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<ForgotPasswordNotification> ForgotPasswordAsync(ForgotPasswordCommand command, CancellationToken cancellationToken = default)
    {
        var user = await _userService.GetAsync(user => user.Email == command.Email, cancellationToken);
        if (user is null) throw new NotFoundException(nameof(User), command.Email);
        var resetLink = await _passwordResetTokenService.CreateAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return new ForgotPasswordNotification(user.Email, resetLink);
    }

    public async Task ResetPasswordAsync(ResetPasswordCommand command, CancellationToken cancellationToken = default)
    {
        var resetPasswordToken = await _passwordResetTokenService.GetAsync(r => r.Token == command.Token, cancellationToken);
        if (resetPasswordToken is null) throw new UnauthorizedAccessException();
        HashPassword(command.NewPassword, out var hashed, out var salt);
        _userService.UpdatePassword(resetPasswordToken.User, hashed, salt);
        _passwordResetTokenService.MarkAsUsed(resetPasswordToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private static void HashPassword(string password, out byte[] hashed, out byte[] salted)
    {
        using var hmac = new HMACSHA512();
        salted = hmac.Key;
        hashed = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
    }

    private static bool VerifyPassword(string password, byte[] hashed, byte[] salted)
    {
        using var hmac = new HMACSHA512(salted);
        var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        return computedHash.SequenceEqual(hashed);
    }
}