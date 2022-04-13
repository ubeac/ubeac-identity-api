using System;

namespace API;

public class UpdateRoleRequest<TKey> where TKey : IEquatable<TKey>
{
    public TKey Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}

public class UpdateRoleRequest : UpdateRoleRequest<Guid>
{
}