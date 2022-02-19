using System.Collections.Generic;
using System.Reflection;
using API;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using uBeac.Repositories.MongoDB;
using uBeac.Web;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

// Adding json config files
builder.Configuration.AddJsonConfig(builder.Environment);

// Get options from configuration files
var emailOptions = builder.Configuration.GetInstance<EmailProviderOptions>("Email");
var jwtOptions = builder.Configuration.GetInstance<JwtOptions>("Jwt");

// Adding swagger
builder.Services.AddCoreSwaggerWithJWT("Example");

// Adding mongodb
builder.Services.AddMongo<MongoDBContext>("DefaultConnection", builder.Environment.IsEnvironment("Testing"));

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
        options.DefaultValues = new List<AppRole> { new("ADMIN") };
    })
    .AddIdentityUnit<AppUnit>()
    .AddIdentityUnitType<AppUnitType>();

// Adding jwt authentication
builder.Services.AddJwtAuthentication(jwtOptions);

var app = builder.Build();
app.UseHttpsRedirection();
app.UseMiddleware<AuthenticationMiddleware>();
app.UseDefaultFiles();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseCoreSwagger();
app.Run();

// For access test projects
public partial class Program { }