namespace API;

// Inherits from Role
public class AppRole : Role
{
    public AppRole() { }
    public AppRole(string name) : base(name) { }

    // You can add your custom properties:
    // public string YourCustomProperty { get; set; }

    // Also, Override base properties:
    // public override Guid Id { get; set; }
}