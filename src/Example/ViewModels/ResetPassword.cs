using System.ComponentModel.DataAnnotations;

namespace Example;

public class ResetPasswordRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}