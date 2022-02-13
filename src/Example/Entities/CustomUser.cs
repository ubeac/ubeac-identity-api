namespace Example;

// Inherits from User
public class CustomUser : User
{
    public CustomUser() { }
    public CustomUser(string userName) : base(userName) { }

    // You can add your custom properties:
    // public string YourCustomProperty { get; set; }

    // Also, Override base properties:
    // public override Guid Id { get; set; }
}