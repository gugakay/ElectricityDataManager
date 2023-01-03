using DataAccess;
using ElectricityDataManager.Infrastructure.BackgroundWorker;
using ElectricityDataManager.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
ConfigurationManager configuration = builder.Configuration;
IWebHostEnvironment environment = builder.Environment;

builder.Services.AddDbContext<DefaultDbContext>(opt =>
        opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<DbContext, DefaultDbContext>();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddHttpClient();

builder.Services.AddTransient<HttpClient>();

builder.Services.AddHostedService<QueuedHostedService>();
builder.Services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddTransient<IFileService, FileService>();
builder.Services.AddTransient<IElectricityService, ElectricityService>();

var redis = ConnectionMultiplexer.Connect(builder.Configuration.GetValue<string>("Redis:ConnectionString"));
builder.Services.AddScoped(s => redis.GetDatabase());

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var _logger = new LoggerConfiguration()
                    .WriteTo.File(Path.Combine(environment.ContentRootPath, "Logs/Log.log"), rollingInterval: RollingInterval.Day)
                    .CreateLogger();

builder.Host.UseSerilog(_logger);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<DefaultDbContext>();
    context.Database.Migrate();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseDeveloperExceptionPage();
app.MapControllers();

app.Run();
 