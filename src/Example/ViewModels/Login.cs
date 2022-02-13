using System;
using System.ComponentModel.DataAnnotations;

namespace Example;

public class LoginRequest
{
    [Required]
    public string UserName { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}

public class LoginResponse
{
    public Guid Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Token { get; set; }
    public string RefreshToken { get; set; }
    public DateTime Expiry { get; set; }
}