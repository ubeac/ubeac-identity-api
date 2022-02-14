using System;
using System.ComponentModel.DataAnnotations;

namespace Example;

public class RegisterRequest
{
    [Required]
    public string UserName { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}

public class RegisterResponse<TUserKey> where TUserKey : IEquatable<TUserKey>
{
    public TUserKey Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Token { get; set; }
    public DateTime Expires { get; set; }
}