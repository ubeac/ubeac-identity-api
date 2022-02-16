using API;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using uBeac.Repositories.MongoDB;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();

// Adding json config files
builder.Configuration.AddJsonConfig(builder.Environment);

// Adding swagger
builder.Services.AddCoreSwaggerWithJWT("Example");

// Adding mongodb
if (builder.Environment.IsEnvironment("Testing"))
    builder.Services.AddMongo<MongoDBContext>("TestConnection");
else
    builder.Services.AddMongo<MongoDBContext>("DefaultConnection");

// Adding application context
builder.Services.AddScoped<IApplicationContext, CustomApplicationContext>();

// Adding repositories
builder.Services.AddMongoDBUserRepository<MongoDBContext, CustomUser>();
builder.Services.AddMongoDBUserTokenRepository<MongoDBContext>();
builder.Services.AddMongoDBRoleRepository<MongoDBContext, CustomRole>();
builder.Services.AddMongoDBUnitRepository<MongoDBContext, CustomUnit>();
builder.Services.AddMongoDBUnitTypeRepository<MongoDBContext, CustomUnitType>();
builder.Services.AddMongoDBUnitRoleRepository<MongoDBContext, CustomUnitRole>();

// Adding services
builder.Services.AddUserService<CustomUserService, CustomUser>();
builder.Services.AddRoleService<CustomRoleService, CustomRole>();
builder.Services.AddUserRoleService<CustomUserRoleService, CustomUser>();
builder.Services.AddUnitService<CustomUnitService, CustomUnit>();
builder.Services.AddUnitTypeService<CustomUnitTypeService, CustomUnitType>();
builder.Services.AddUnitRoleService<CustomUnitRoleService, CustomUnitRole>();

// Adding identity core
builder.Services
    .AddIdentityUser<CustomUser>(configureOptions: options =>
    {
        options.AdminUser = new CustomUser("admin");
        options.AdminPassword = "1qaz!QAZ";
        options.AdminRole = "ADMIN";
    })
    .AddIdentityRole<CustomRole>()
    .AddIdentityUnit<CustomUnit>()
    .AddIdentityUnitType<CustomUnitType>();

// Adding jwt authentication
builder.Services.AddJwtAuthentication(builder.Configuration.GetInstance<JwtOptions>("Jwt"));

var app = builder.Build();
app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();
app.UseAuthorization();
app.MapControllers();
app.UseCoreSwagger();
app.Run();

// For access test projects
public partial class Program { }