using System.ComponentModel.DataAnnotations;

namespace Example;

public class RefreshTokenRequest
{
    [Required]
    public virtual string Token { get; set; }

    [Required]
    public virtual string RefreshToken { get; set; }
}