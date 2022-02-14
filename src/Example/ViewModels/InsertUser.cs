using System.ComponentModel.DataAnnotations;

namespace Example;

public class InsertUserRequest
{
    [Required]
    public virtual string UserName { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public virtual string Password { get; set; }

    public virtual string PhoneNumber { get; set; }
    public virtual bool PhoneNumberConfirmed { get; set; }

    public virtual string Email { get; set; }
    public virtual bool EmailConfirmed { get; set; }
}