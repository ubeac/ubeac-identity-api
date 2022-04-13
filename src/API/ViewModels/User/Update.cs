using System;
using System.ComponentModel.DataAnnotations;

namespace API;

// Update user info by admin
public class UpdateUserRequest<TKey> where TKey : IEquatable<TKey>
{
    [Required]
    public virtual TKey Id { get; set; }

    [Required]
    public virtual string FirstName { get; set; }

    [Required]
    public virtual string LastName { get; set; }

    public virtual string PhoneNumber { get; set; }
    public virtual bool PhoneNumberConfirmed { get; set; }

    public virtual string Email { get; set; }
    public virtual bool EmailConfirmed { get; set; }

    public virtual bool LockoutEnabled { get; set; }
    public virtual DateTimeOffset? LockoutEnd { get; set; }

    public virtual bool Enabled { get; set; }
}

public class UpdateUserRequest : UpdateUserRequest<Guid>
{
}

// Update info of authenticated user
public class UpdateAccountRequest
{
    public virtual string FirstName { get; set; }
    public virtual string LastName { get; set; }

    public virtual string PhoneNumber { get; set; }
    public virtual string Email { get; set; }
}