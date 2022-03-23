using System;

namespace API;

// Inherits from User
public class AppUser : User, IAuditEntity
{
    public AppUser() { }
    public AppUser(string userName) : base(userName) { }

    public virtual string FirstName { get; set; }
    public virtual string LastName { get; set; }
    public virtual string CreatedBy { get; set; }
    public virtual string CreatedByIp { get; set; }
    public virtual DateTime CreatedAt { get; set; }
    public virtual string LastUpdatedBy { get; set; }
    public virtual string LastUpdatedByIp { get; set; }
    public virtual DateTime? LastUpdatedAt { get; set; }
}