using System;

namespace API;

public class RoleResponse<TKey> where TKey : IEquatable<TKey>
{
    public TKey Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}

public class RoleResponse : RoleResponse<Guid>
{
}