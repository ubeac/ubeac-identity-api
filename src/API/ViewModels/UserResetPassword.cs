using System.ComponentModel.DataAnnotations;

namespace API;

public class ResetPasswordRequest
{
    [Required]
    public string UserName { get; set; }

    [Required] 
    public string Token { get; set; }

    [Required] 
    [DataType(DataType.Password)]
    public string NewPassword { get; set; }
}