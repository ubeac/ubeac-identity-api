using System.ComponentModel.DataAnnotations;

namespace Example;

public class ForgotPasswordRequest
{
    [Required]
    public virtual string UserName { get; set; }
}