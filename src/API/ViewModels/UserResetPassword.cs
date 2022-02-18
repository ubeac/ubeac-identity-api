using System.ComponentModel.DataAnnotations;

namespace API;

public class ResetPasswordRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}