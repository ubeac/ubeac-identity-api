using System;
using System.ComponentModel.DataAnnotations;

namespace API;

public class ChangePasswordRequest<TKey> where TKey : IEquatable<TKey>
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

public class ChangePasswordRequest : ChangePasswordRequest<Guid>
{
}