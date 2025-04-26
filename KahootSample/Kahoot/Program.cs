using DAO;
using Kahoot.Hubs;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Repositories;
using Repositories.IRepository;
using Repositories.Repository;
using Services;
using Services.IService;
using Services.Mapping;
using Services.Service;

using StackExchange.Redis;


var builder = WebApplication.CreateBuilder(args);
//Register Redis Service
//builder.Services.AddSingleton<RedisCacheService>();
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<KahootDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection"),
    sqlOptions => sqlOptions.MigrationsAssembly("DAO")));
// Configure AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddSignalR();
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("localhost:6379"));

// Configure Services using extension methods
builder.Services.ConfigureDataAccessObjectService(builder.Configuration);
builder.Services.ConfigureRepositoryService(builder.Configuration);
builder.Services.ConfigureServiceService(builder.Configuration);

// Add SwaggerGen for Authentication
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Kahoot API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[]{}
        }
    });
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.WithOrigins("http://localhost:5173", "https://localhost:7221")
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials();
    });
});

builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("AllowAll");
app.UseAuthorization();

app.MapControllers();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<GameSessionHub>("/gameSessionHub"); // Đăng ký SignalR Hub với endpoint /gameSessionHub
});

// Đăng ký Redis
//builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
//{
//    var configuration = builder.Configuration.GetSection("Redis:ConnectionString").Value;
//    return ConnectionMultiplexer.Connect(configuration);
//});

app.Run();
