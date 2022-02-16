using System.Net;
using Microsoft.AspNetCore.Http;
using uBeac.Web;

namespace API;

// Inherits from ApplicationContext
public class CustomApplicationContext : ApplicationContext
{
    public CustomApplicationContext(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
    {
    }

    // You can add your custom properties:
    // public string YourCustomProperty { get; set; }
}