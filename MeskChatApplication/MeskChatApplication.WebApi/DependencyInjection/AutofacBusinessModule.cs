using Autofac;
using MeskChatApplication.Application.Services;
using MeskChatApplication.Domain.Repositories;
using MeskChatApplication.Infrastructure.Services;
using MeskChatApplication.Persistance.Repositories;
using MeskChatApplication.Persistance.Services;

namespace MeskChatApplication.WebApi.DependencyInjection;

public class AutofacBusinessModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();
        
        builder.RegisterType<UserRepository>().As<IUserRepository>().InstancePerLifetimeScope();
        builder.RegisterType<RefreshTokenRepository>().As<IRefreshTokenRepository>().InstancePerLifetimeScope();
        builder.RegisterType<PasswordResetTokenRepository>().As<IPasswordResetTokenRepository>().InstancePerLifetimeScope();
        builder.RegisterType<MessageRepository>().As<IMessageRepository>().InstancePerLifetimeScope();
        
        builder.RegisterType<EmailService>().As<IEmailService>().InstancePerLifetimeScope();
        builder.RegisterType<FileStorageService>().As<IFileStorageService>().InstancePerLifetimeScope();
        
        builder.RegisterType<RefreshTokenService>().As<IRefreshTokenService>().InstancePerLifetimeScope();
        builder.RegisterType<UserService>().As<IUserService>().InstancePerLifetimeScope();
        builder.RegisterType<AuthenticationService>().As<IAuthenticationService>().InstancePerLifetimeScope();
        builder.RegisterType<JwtService>().As<IJwtService>().InstancePerLifetimeScope();
        builder.RegisterType<PasswordResetTokenService>().As<IPasswordResetTokenService>().InstancePerLifetimeScope();
        builder.RegisterType<MessageService>().As<IMessageService>().InstancePerLifetimeScope();
    }
}