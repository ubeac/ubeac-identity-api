using System;

namespace API;

public class RoleResponse<TKey> where TKey : IEquatable<TKey>
{
    public virtual TKey Id { get; set; }
    public virtual string Name { get; set; }
    public virtual string Description { get; set; }
}

public class RoleResponse : RoleResponse<Guid>
{
}