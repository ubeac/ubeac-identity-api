using System;
using System.Collections.Generic;
using System.Reflection;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using uBeac.Logging.MongoDB;

var builder = WebApplication.CreateBuilder(args);

// Adding json config files
builder.Configuration.AddJsonConfig(builder.Environment);

// Adding logger
var logger = new LoggerConfiguration()
    .WriteTo.Logger(_ => _.AddDefaultLogging(builder.Services).WriteToMongoDB(builder.Configuration.GetConnectionString("DefaultLogsConnection")))
    .WriteTo.Logger(_ => _.AddHttpLogging(builder.Services).WriteToMongoDB(builder.Configuration.GetConnectionString("HttpLogsConnection")))
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

builder.Services.AddHttpContextAccessor();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddControllers()
    .AddFluentValidation(_ => _.RegisterValidatorsFromAssemblyContaining<DummyValidator>());

// Adding CORS policy
const string DefaultCorsPolicy = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(DefaultCorsPolicy, policy =>
    {
        policy.AllowAnyHeader()
            .AllowAnyMethod()
            .AllowAnyOrigin();
    });
});

// Disabling automatic model state validation of ASP.NET Core
builder.Services.DisableAutomaticModelStateValidation();

// Adding debugger
builder.Services.AddDebugger();

// Adding swagger
builder.Services.AddCoreSwaggerWithJWT("Example");

// Adding mongodb
builder.Services.AddMongo<MongoDBContext>("DefaultConnection")
    .AddDefaultBsonSerializers();

// Adding application context
builder.Services.AddApplicationContext();

// Adding history
builder.Services.AddHistory().UsingMongoDb().ForDefault();

// Adding email provider
builder.Services.AddEmailProvider(builder.Configuration);

// Adding repositories
builder.Services.AddMongoDBUserRepository<MongoDBContext, AppUser>();
builder.Services.AddMongoDBUserTokenRepository<MongoDBContext>();
builder.Services.AddMongoDBRoleRepository<MongoDBContext, AppRole>();

// Adding services
builder.Services.AddUserService<UserService<AppUser>, AppUser>();
builder.Services.AddRoleService<RoleService<AppRole>, AppRole>();
builder.Services.AddUserRoleService<UserRoleService<AppUser>, AppUser>();

// Adding jwt service
builder.Services.AddJwtService<AppUser>(builder.Configuration);

// Adding authentication
builder.Services.AddJwtAuthentication(builder.Configuration);

// Adding identity core
builder.Services
    .AddIdentityUser<AppUser>(configureOptions: options =>
    {
        options.AdminUser = new AppUser("admin");
        options.AdminPassword = "1qaz!QAZ";
        options.AdminRole = "ADMIN";
    })
    .AddIdentityRole<AppRole>(configureOptions: options =>
    {
        options.DefaultValues = new List<AppRole>
        {
            new("ADMIN"), new("ACCOUNTING_ADMIN"), new("FLIGHT_ADMIN"), new("FLIGHT_AGENT"), new("FLIGHT_TICKETING")
        };
    });

// Adding HSTS
var hstsOptions = builder.Configuration.GetSection("Hsts").Get<HstsOptions>();
builder.Services.AddHsts(options =>
{
    options.Preload = hstsOptions.Preload;
    options.IncludeSubDomains = hstsOptions.IncludeSubDomains;
    options.MaxAge = TimeSpan.FromDays(hstsOptions.MaxAge);
    foreach (var host in hstsOptions.ExcludedHosts) options.ExcludedHosts.Add(host);
});

var app = builder.Build();

if (app.Environment.IsProduction())
{
    app.UseHsts();
    app.UseHttpsRedirection();
}

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseApiLogging();

app.MapControllers();

app.UseCors(DefaultCorsPolicy);

app.UseAuthentication();
app.UseAuthorization();

app.UseCoreSwagger();
app.Run();

// For access test projects
public partial class Program { }