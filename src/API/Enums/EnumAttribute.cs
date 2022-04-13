using System;

namespace uBeac.Enums;

[AttributeUsage(AttributeTargets.Enum)]
public class EnumAttribute : Attribute
{
    public string Description { get; set; }

    public EnumAttribute(string description)
    {
        Description = description;
    }

    public EnumAttribute()
    {
    }
}