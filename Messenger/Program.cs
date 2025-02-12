using Messenger.Data;
using Messenger.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();
builder.Services.AddMvc();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

var connectionString = configuration.GetConnectionString("DefaultConnection");

builder.Services.AddScoped<MessageRepository>(provider =>
    new MessageRepository(connectionString));

var app = builder.Build();

app.MapHub<MessageHub>("/messageHub");

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

var dbContext = new ApplicationContext(connectionString);
await dbContext.CreateMessagesTableAsync();

app.Run();
