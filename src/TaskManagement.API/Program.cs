using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
using System.Threading.RateLimiting;
using TaskManagement.Application.Interfaces;
using TaskManagementSystem.API.Middleware;
using TaskManagementSystem.Application.Common.Mappings;
using TaskManagementSystem.Application.Interfaces;
using TaskManagementSystem.Application.Validators;
using TaskManagementSystem.Infrastructure.Data;
using TaskManagementSystem.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

#region Serilog Configuration
//Configure Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File(
        "logs/log-.txt",
        rollingInterval: RollingInterval.Day,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] {Message}{NewLine}{Exception}"
    )
    .CreateLogger();


builder.Host.UseSerilog();

#endregion

//builder.Services.AddAutoMapper(config => config.AddProfile<MappingProfile>());
var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
builder.Services.AddSingleton(mapperConfig);
builder.Services.AddSingleton(mapperConfig.CreateMapper());

builder.Services.AddControllers()
    .AddFluentValidation(fv =>
        fv.RegisterValidatorsFromAssemblyContaining<CreateTaskValidator>());

// Add DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<IAdminService, AdminService>();



builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });

#region AutoMapper Configuration
builder.Services.AddRateLimiter(options =>
{
    // Global policy
    options.AddFixedWindowLimiter("fixed", opt =>
    {
        opt.PermitLimit = 10; // max requests
        opt.Window = TimeSpan.FromSeconds(10); // per 10 seconds
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        opt.QueueLimit = 2;
    });

    options.RejectionStatusCode = 429; // Too Many Requests
    options.OnRejected = async (context, token) =>
    {
        context.HttpContext.Response.ContentType = "application/json";

        await context.HttpContext.Response.WriteAsync(
            "{\"message\": \"Too many requests. Please try again later.\"}",
            cancellationToken: token
        );
    };
});
#endregion

builder.Services.AddAuthorization();


builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Log application start
Log.Information("Application started");

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseRateLimiter();

 // run migration will create db 
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    await DbInitializer.InitializeAsync(dbContext);
}

app.Run();
