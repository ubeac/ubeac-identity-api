using System.ComponentModel.DataAnnotations;

namespace API;

public class ForgotPasswordRequest
{
    [Required]
    public virtual string UserName { get; set; }
}