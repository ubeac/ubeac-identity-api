using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API;

public class LoginRequest
{
    [Required]
    public string UserName { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}

public class LoginResponse<TUserKey>
{
    public virtual TUserKey UserId { get; set; }
    public virtual List<string> Roles { get; set; }
    public virtual string Token { get; set; }
    public virtual string RefreshToken { get; set; }
    public virtual DateTime Expiry { get; set; }
}

public class LoginResponse : LoginResponse<Guid>
{
}