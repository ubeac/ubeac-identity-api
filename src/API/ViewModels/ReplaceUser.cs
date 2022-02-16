using System;
using System.ComponentModel.DataAnnotations;

namespace API;

public class ReplaceUserRequest<TKey> where TKey : IEquatable<TKey>
{
    [Required]
    public virtual TKey Id { get; set; }

    public virtual string PhoneNumber { get; set; }
    public virtual bool PhoneNumberConfirmed { get; set; }

    public virtual string Email { get; set; }
    public virtual bool EmailConfirmed { get; set; }

    public virtual bool LockoutEnabled { get; set; }
    public virtual DateTimeOffset? LockoutEnd { get; set; }
}

public class ReplaceUserRequest : ReplaceUserRequest<Guid>
{
}