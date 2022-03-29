using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AuthenticationMiddleware = uBeac.Identity.AuthenticationMiddleware;
using AuthenticationOptions = uBeac.Identity.AuthenticationOptions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

// Adding json config files
builder.Configuration.AddJsonConfig(builder.Environment);

// Get options from configuration files
var emailOptions = builder.Configuration.GetInstance<EmailProviderOptions>("Email");
var jwtOptions = builder.Configuration.GetInstance<JwtOptions>("Jwt");

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

// Disabling automatic model state validation by ASP.NET Core
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

// Adding debugger
builder.Services.AddDebugger();

// Adding swagger
builder.Services.AddCoreSwaggerWithJWT("Example");

// Adding mongodb
builder.Services.AddMongo<MongoDBContext>("DefaultConnection")
    .AddDefaultBsonSerializers();

// Adding application context
builder.Services.AddScoped<IApplicationContext, ApplicationContext>();

// Adding email provider
builder.Services.AddEmailProvider(emailOptions);

// Adding repositories
builder.Services.AddMongoDBUserRepository<MongoDBContext, AppUser>();
builder.Services.AddMongoDBUserTokenRepository<MongoDBContext>();
builder.Services.AddMongoDBRoleRepository<MongoDBContext, AppRole>();
builder.Services.AddMongoDBUnitRepository<MongoDBContext, AppUnit>();
builder.Services.AddMongoDBUnitTypeRepository<MongoDBContext, AppUnitType>();
builder.Services.AddMongoDBUnitRoleRepository<MongoDBContext, AppUnitRole>();

// Adding services
builder.Services.AddUserService<UserService<AppUser>, AppUser>();
builder.Services.AddRoleService<RoleService<AppRole>, AppRole>();
builder.Services.AddUserRoleService<UserRoleService<AppUser>, AppUser>();
builder.Services.AddUnitService<UnitService<AppUnit>, AppUnit>();
builder.Services.AddUnitTypeService<UnitTypeService<AppUnitType>, AppUnitType>();
builder.Services.AddUnitRoleService<UnitRoleService<AppUnitRole>, AppUnitRole>();

// Adding jwt service
builder.Services.AddJwtService<AppUser>(jwtOptions);

// Adding authentication
builder.Services.AddAuthentication<LocalAuthenticator<AppUser>>(new AuthenticationOptions
{
    Issuer = jwtOptions.Issuer,
    Audience = jwtOptions.Audience,
    Secret = jwtOptions.Secret
});

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
    })
    .AddIdentityUnit<AppUnit>()
    .AddIdentityUnitType<AppUnitType>();

var app = builder.Build();

// app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();

app.MapControllers();

app.UseCors(DefaultCorsPolicy);

app.UseAuthentication();
app.UseMiddleware<AuthenticationMiddleware>(); // This middleware should be called after UseAuthentication
app.UseAuthorization();

app.UseCoreSwagger();
app.Run();

// For access test projects
public partial class Program { }