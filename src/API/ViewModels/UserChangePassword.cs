using System;
using System.ComponentModel.DataAnnotations;

namespace API;

// Change user password by admin
public class ChangeUserPasswordRequest<TKey> where TKey : IEquatable<TKey>
{
    [Required]
    public virtual TKey UserId { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public virtual string CurrentPassword { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public virtual string NewPassword { get; set; }
}

public class ChangeUserPasswordRequest : ChangeUserPasswordRequest<Guid>
{
}

// Change password of authenticated user
public class ChangeAccountPasswordRequest
{
    [Required]
    [DataType(DataType.Password)]
    public string CurrentPassword { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string NewPassword { get; set; }
}