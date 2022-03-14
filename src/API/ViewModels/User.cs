using System;

namespace API;

public class UserResponse<TKey> where TKey : IEquatable<TKey>
{
    public virtual TKey Id { get; set; }
    public virtual string UserName { get; set; }
    public virtual string FirstName { get; set; }
    public virtual string LastName { get; set; }
    public virtual string Email { get; set; }
    public virtual bool EmailConfirmed { get; set; }
    public virtual string PhoneNumber { get; set; }
    public virtual bool PhoneNumberConfirmed { get; set; }
    public virtual DateTime CreatedAt { get; set; }
    public virtual DateTime LastUpdatedAt { get; set; }
}

public class UserResponse : UserResponse<Guid>
{
}