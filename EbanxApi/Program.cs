var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IAccountService, AccountService>();

builder.Services.AddControllers(); 

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
