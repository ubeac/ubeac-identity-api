namespace API;

// Inherits from User
public class AppUser : User
{
    public AppUser() { }
    public AppUser(string userName) : base(userName) { }

    // You can add your custom properties:
    // public string YourCustomProperty { get; set; }

    // Also, Override base properties:
    // public override Guid Id { get; set; }
}