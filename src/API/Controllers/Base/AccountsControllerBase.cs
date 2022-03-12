using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Omu.ValueInjecter;
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
    protected readonly IMapper Mapper;

    protected AccountsControllerBase(IUserService<TUserKey, TUser> userService, IMapper mapper)
    {
        UserService = userService;
        Mapper = mapper;
    }

    /// <summary>
    /// Registers a new user
    /// </summary>
    /// <returns>If an exception is thrown, returns false, otherwise true</returns>
    [HttpPost]
    public virtual async Task<IApiResult<RegisterResponse<TUserKey>>> Register([FromBody] RegisterRequest model, CancellationToken cancellationToken = default)
    {
        try
        {
            await UserService.Register(model.UserName, model.Email, model.Password, cancellationToken);
            var authResult = await UserService.Authenticate(model.UserName, model.Password, cancellationToken);
            var response = Mapper.Map<RegisterResponse<TUserKey>>(authResult);
            return response.ToApiResult();
        }
        catch (Exception ex)
        {
            return ex.ToApiResult<RegisterResponse<TUserKey>>();
        }
    }

    /// <summary>
    /// Generates user credential (like access token, refresh token, etc.)
    /// </summary>
    /// <returns>Returns user credential (like access token, refresh token, etc.)</returns>
    [HttpPost]
    public virtual async Task<IApiResult<LoginResponse<TUserKey>>> Login([FromBody] LoginRequest model, CancellationToken cancellationToken = default)
    {
        try
        {
            var authResult = await UserService.Authenticate(model.UserName, model.Password, cancellationToken);
            var response = Mapper.Map<LoginResponse<TUserKey>>(authResult);
            return response.ToApiResult();
        }
        catch (Exception ex)
        {
            return ex.ToApiResult<LoginResponse<TUserKey>>();
        }
    }

    [HttpPost]
    public virtual async Task<IApiResult<bool>> Logout(CancellationToken cancellationToken = default)
    {
        try
        {
            var userId = await UserService.GetCurrentUserId(cancellationToken);
            // TODO: Refactor this: Only one token (current session) should be revoked
            await UserService.RevokeTokens(userId, cancellationToken);
            return true.ToApiResult();
        }
        catch (Exception ex)
        {
            return ex.ToApiResult<bool>();
        }
    }

    /// <summary>
    /// Refreshes user credential (like access token, refresh token, etc.)
    /// </summary>
    /// <returns>Returns refreshed user credential (like access token, refresh token, etc.)</returns>
    [HttpPost]
    public virtual async Task<IApiResult<LoginResponse<TUserKey>>> RefreshToken([FromBody] RefreshTokenRequest model, CancellationToken cancellationToken = default)
    {
        try
        {
            var authResult = await UserService.RefreshToken(model.RefreshToken, model.Token, cancellationToken);
            var response = Mapper.Map<LoginResponse<TUserKey>>(authResult);
            return response.ToApiResult();
        }
        catch (Exception ex)
        {
            return ex.ToApiResult<LoginResponse<TUserKey>>();
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

    /// <summary>
    /// Get info of current user
    /// </summary>
    /// <returns>Returns user info</returns>
    [HttpGet]
    [Authorize]
    public virtual async Task<IApiResult<UserResponse<TUserKey>>> GetCurrent(CancellationToken cancellationToken = default)
    {
        try
        {
            // TODO: Refactor this: (one query instead of two query -- get current user)
            var userId = await UserService.GetCurrentUserId(cancellationToken);
            var user = await UserService.GetById(userId, cancellationToken); var userVm = Mapper.Map<UserResponse<TUserKey>>(user);
            return userVm.ToApiResult();
        }
        catch (Exception ex)
        {
            return ex.ToApiResult<UserResponse<TUserKey>>();
        }
    }

    /// <summary>
    /// Changes account info
    /// </summary>
    /// <returns>If an exception is thrown, returns false, otherwise true</returns>
    [HttpPost]
    [Authorize]
    public virtual async Task<IApiResult<bool>> Update([FromBody] UpdateAccountRequest model, CancellationToken cancellationToken = default)
    {
        try
        {
            // TODO: Refactor this: (one query instead of two query -- get current user)
            var userId = await UserService.GetCurrentUserId(cancellationToken);
            var user = await UserService.GetById(userId, cancellationToken);
            user.InjectFrom(model);
            await UserService.Update(user, cancellationToken);
            return true.ToApiResult();

        }
        catch (Exception ex)
        {
            return ex.ToApiResult<bool>();
        }
    }

    /// <summary>
    /// Changes account password
    /// </summary>
    /// <returns>If an exception is thrown, returns false, otherwise true</returns>
    [HttpPost]
    [Authorize]
    public virtual async Task<IApiResult<bool>> ChangePassword([FromBody] ChangeAccountPasswordRequest model, CancellationToken cancellationToken = default)
    {
        try
        {
            var changePassword = Mapper.Map<ChangePassword<TUserKey>>(model);
            changePassword.UserId = await UserService.GetCurrentUserId(cancellationToken);
            await UserService.ChangePassword(changePassword, cancellationToken);
            return true.ToApiResult();
        }
        catch (Exception ex)
        {
            return ex.ToApiResult<bool>();
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
    protected AccountsControllerBase(IUserService<TUser> userService, IMapper mapper) : base(userService, mapper)
    {
    }
}
