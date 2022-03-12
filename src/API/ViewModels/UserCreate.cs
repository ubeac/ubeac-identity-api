using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API;

public class CreateUserRequest
{
    [Required]
    public virtual string UserName { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public virtual string Password { get; set; }

    [Required]
    public virtual string FirstName { get; set; }

    [Required]
    public virtual string LastName { get; set; }

    public virtual string PhoneNumber { get; set; }
    public virtual bool PhoneNumberConfirmed { get; set; }

    public virtual string Email { get; set; }
    public virtual bool EmailConfirmed { get; set; }

    public virtual List<string> Roles { get; set; }
}