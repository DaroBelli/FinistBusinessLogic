using FinistBusinessLogic.Controllers;
using FinistBusinessLogic.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();

var app = builder.Build();

app.MapGrpcService<UserApiService>();
app.MapGrpcService<LoginningApiService>();

app.Run(ConfigJSON.GetConfig().GetSection("Address").Value);
