using System.ComponentModel.DataAnnotations;

namespace API;

public class RefreshTokenRequest
{
    [Required]
    public virtual string Token { get; set; }

    [Required]
    public virtual string RefreshToken { get; set; }
}