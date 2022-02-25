using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API;

public class AssignRoleRequest<TKey> where TKey : IEquatable<TKey>
{
    [Required]
    public virtual TKey Id { get; set; }

    public List<string> Roles { get; set; }
}

public class AssignRoleRequest : AssignRoleRequest<Guid>
{
}