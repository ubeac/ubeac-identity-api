using Microsoft.AspNetCore.Cors.Infrastructure;

public class CorsPolicyOptions : CorsPolicy
{
    public string Name { get; set; }
}