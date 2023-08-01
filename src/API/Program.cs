using System.Collections.Generic;
using System.Reflection;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using uBeac.Repositories.History.MongoDB;
using uBeac.Web.Logging;
using uBeac.Web.Logging.MongoDB;

var builder = WebApplication.CreateBuilder(args);

// Adding json config files
builder.Configuration.AddJsonConfig(builder.Environment);

// Adding http logging
builder.Services.AddMongoDbHttpLogging<HttpLogMongoDBContext>("HttpLoggingConnection", builder.Configuration.GetInstance<MongoDbHttpLogOptions>("HttpLogging"));

builder.Services.AddHttpContextAccessor();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();

// Adding CORS policy
var corsPolicyOptions = builder.Configuration.GetSection("CorsPolicy");
builder.Services.AddCorsPolicy(corsPolicyOptions);

// Adding HSTS
var hstsOptions = builder.Configuration.GetSection("Hsts");
builder.Services.AddHttpsPolicy(hstsOptions);

// Disabling automatic model state validation of ASP.NET Core
builder.Services.DisableAutomaticModelStateValidation();

// Adding debugger
builder.Services.AddDebugger();

// Adding swagger
builder.Services.AddCoreSwaggerWithJWT("Example");

// Adding mongodb
builder.Services.AddMongo<MongoDBContext>("DefaultConnection");

// Adding application context
builder.Services.AddApplicationContext();

// Adding history
builder.Services.AddMongo<HistoryMongoDBContext>("HistoryConnection");
builder.Services.AddHistory<MongoDBHistoryRepository>().For<AppUser>();

// Adding email provider
builder.Services.AddEmailProvider(builder.Configuration);

// Adding repositories
builder.Services.AddMongoDBUserRepository<MongoDBContext, AppUser>();
builder.Services.AddMongoDBUserTokenRepository<MongoDBContext>();
builder.Services.AddMongoDBRoleRepository<MongoDBContext, AppRole>();

// Adding services
builder.Services.AddUserService<UserService<AppUser>, AppUser>(builder.Configuration);
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

var app = builder.Build();

app.UseHttpsRedirection();
app.UseHstsOnProduction(builder.Environment);
app.UseCorsPolicy(corsPolicyOptions);

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseCoreSwagger();

app.UseAuthentication();
app.UseHttpLoggingMiddleware();
app.UseAuthorization();

app.MapControllers();
app.Run();

// For access test projects
public partial class Program { }