using System.Text.Json.Serialization;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using FluentValidation;
using MESK.MediatR;
using MESK.MiniEndpoint;
using MeskChatApplication.Application.Behaviors;
using MeskChatApplication.Persistance.Context;
using MeskChatApplication.Presentation.Hubs;
using MeskChatApplication.WebApi.DependencyInjection;
using MeskChatApplication.WebApi.Extensions;
using MeskChatApplication.WebApi.Middlewares;
using MeskChatApplication.WebApi.Options.ApplicationSetup;
using MeskChatApplication.WebApi.Options.EmailSetup;
using MeskChatApplication.WebApi.Options.JwtSetup;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
builder.Services.AddAuthorization();

builder.Services.ConfigureOptions<JwtOptionsSetup>();
builder.Services.ConfigureOptions<JwtBearerOptionsSetup>();
builder.Services.ConfigureOptions<EmailOptionsSetup>();
builder.Services.ConfigureOptions<ApplicationOptionsSetup>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDatabaseContext>(opts =>
{
    opts.UseNpgsql(connectionString);
});

builder.Services.AddValidatorsFromAssembly(typeof(MeskChatApplication.Application.AssemblyReference).Assembly);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterModule<AutofacBusinessModule>();
});

builder.Services.AddMediatR(opts =>
{
    opts.RegisterServicesFromAssembly(typeof(MeskChatApplication.Application.AssemblyReference).Assembly);
    opts.RegisterPipelineBehavior(typeof(TransactionBehavior<,>));
    opts.RegisterPipelineBehavior(typeof(ValidationBehavior<,>));
});

builder.Services.AddMiniEndpoints(typeof(MeskChatApplication.Presentation.AssemblyReference).Assembly);

builder.Services.AddOpenApi();

builder.Services.AddSignalR();

builder.Services.AddRateLimiter();

var app = builder.Build();

app.UseImprovedRateLimiter();

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

var baseGroup = app.MapGroup("/api/v1");

app.MapMiniEndpoints(baseGroup);

app.MapHub<ChatHub>("/chat");

app.MapOpenApi();
app.MapScalarApiReference(opts =>
{
    opts.WithTitle("MeskChatApplication.WebApi")
        .WithTheme(ScalarTheme.DeepSpace)
        .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
});

app.Run();
