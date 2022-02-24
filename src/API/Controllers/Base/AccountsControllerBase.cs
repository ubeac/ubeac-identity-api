using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using uBeac.Web;

namespace API;

/// <summary>
/// This class is an abstract controller (base controller) that has base action methods (endpoints) to accounts management.
/// </summary>
/// <typeparam name="TUserKey">Type of user entity key -- TKey must have inherited from IEquatable</typeparam>
/// <typeparam name="TUser">Type of user entity -- TUser must have inherited from User</typeparam>
public abstract class AccountsControllerBase<TUserKey, TUser> : BaseController
   where TUserKey : IEquatable<TUserKey>
   where TUser : User<TUserKey>
{
    protected readonly IUserService<TUserKey, TUser> UserService;

    protected AccountsControllerBase(IUserService<TUserKey, TUser> userService)
    {
        UserService = userService;
    }

    /// <summary>
    /// Registers a new user
    /// </summary>
    /// <returns>If an exception is thrown, returns false, otherwise true</returns>
    [HttpPost]
    public virtual async Task<IApiResult<bool>> Register([FromBody] RegisterRequest model, CancellationToken cancellationToken = default)
    {
        try
        {
            await UserService.Register(model.UserName, model.Email, model.Password, cancellationToken);
            return true.ToApiResult();
        }
        catch (Exception ex)
        {
            return ex.ToApiResult<bool>();
        }
    }

    /// <summary>
    /// Generates user credential (like access token, refresh token, etc.)
    /// </summary>
    /// <returns>Returns user credential (like access token, refresh token, etc.)</returns>
    [HttpPost]
    public virtual async Task<IApiResult<TokenResult<TUserKey>>> Login([FromBody] LoginRequest model, CancellationToken cancellationToken = default)
    {
        try
        {
            var authResult = await UserService.Authenticate(model.UserName, model.Password, cancellationToken);
            return authResult.ToApiResult();
        }
        catch (Exception ex)
        {
            return ex.ToApiResult<TokenResult<TUserKey>>();
        }
    }

    /// <summary>
    /// Refreshes user credential (like access token, refresh token, etc.)
    /// </summary>
    /// <returns>Returns refreshed user credential (like access token, refresh token, etc.)</returns>
    [HttpPost]
    public virtual async Task<IApiResult<TokenResult<TUserKey>>> RefreshToken([FromBody] RefreshTokenRequest model, CancellationToken cancellationToken = default)
    {
        try
        {
            var authResult = await UserService.RefreshToken(model.RefreshToken, model.Token, cancellationToken);
            return authResult.ToApiResult();
        }
        catch (Exception ex)
        {
            return ex.ToApiResult<TokenResult<TUserKey>>();
        }
    }

    /// <summary>
    /// Sends a forgot password email to user
    /// </summary>
    /// <returns>If an exception is thrown, returns false, otherwise true</returns>
    [HttpPost]
    public virtual async Task<IApiResult<bool>> ForgotPassword([FromBody] ForgotPasswordRequest model, CancellationToken cancellationToken = default)
    {
        try
        {
            await UserService.ForgotPassword(model.UserName, cancellationToken);
            return true.ToApiResult();

        }
        catch (Exception)
        {
            return false.ToApiResult();
        }
    }

    /// <summary>
    /// Resets user password by token and new password
    /// </summary>
    /// <returns>If an exception is thrown, returns false, otherwise true</returns>
    [HttpPost]
    public virtual async Task<IApiResult<bool>> ResetPassword([FromBody] ResetPasswordRequest model, CancellationToken cancellationToken = default)
    {
        try
        {
            await UserService.ResetPassword(model.UserName, model.Token, model.NewPassword, cancellationToken);
            return true.ToApiResult();
        }
        catch (Exception)
        {
            return false.ToApiResult();
        }
    }
}

/// <summary>
/// This class is an abstract controller (base controller) that has base action methods (endpoints) to accounts management.
/// </summary>
/// <typeparam name="TUser">Type of user entity -- TUser must have inherited from User</typeparam>
public abstract class AccountsControllerBase<TUser> : AccountsControllerBase<Guid, TUser>
    where TUser : User
{
    protected AccountsControllerBase(IUserService<TUser> userService) : base(userService)
    {
    }
}
