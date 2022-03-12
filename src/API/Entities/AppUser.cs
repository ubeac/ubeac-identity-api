namespace API;

// Inherits from User
public class AppUser : User
{
    public AppUser() { }
    public AppUser(string userName) : base(userName) { }

    public virtual string FirstName { get; set; }
    public virtual string LastName { get; set; }
}