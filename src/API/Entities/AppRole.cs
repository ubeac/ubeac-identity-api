using System;

namespace API;

// Inherits from Role
public class AppRole : Role, IAuditEntity
{
    public AppRole() { }
    public AppRole(string name) : base(name) { }

    // You can add your custom properties:
    // public string YourCustomProperty { get; set; }

    // Also, Override base properties:
    // public override Guid Id { get; set; }

    public virtual string CreatedBy { get; set; }
    public virtual string CreatedByIp { get; set; }
    public virtual DateTime CreatedAt { get; set; }

    public virtual string LastUpdatedBy { get; set; }
    public virtual string LastUpdatedByIp { get; set; }
    public virtual DateTime? LastUpdatedAt { get; set; }

    public virtual IApplicationContext Context { get; set; }
}