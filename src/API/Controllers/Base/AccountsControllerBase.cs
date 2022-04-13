﻿using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
    protected readonly IUserRoleService<TUserKey, TUser> UserRoleService;
    protected readonly IMapper Mapper;

    protected AccountsControllerBase(IUserService<TUserKey, TUser> userService, IUserRoleService<TUserKey, TUser> userRoleService, IMapper mapper)
    {
        UserService = userService;
        UserRoleService = userRoleService;
        Mapper = mapper;
    }

    /// <summary>
    /// Registers a new user
    /// </summary>
    /// <returns>If an exception is thrown, returns false, otherwise true</returns>
    [HttpPost]
    public virtual async Task<IResult<RegisterResponse<TUserKey>>> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken = default)
    {
        var user = Mapper.Map<TUser>(request);
        await UserService.Create(user, request.Password, cancellationToken);
        var authResult = await UserService.Authenticate(request.UserName, request.Password, cancellationToken);
        var response = Mapper.Map<RegisterResponse<TUserKey>>(authResult);
        return response.ToResult();
    }

    /// <summary>
    /// Generates user credential (like access token, refresh token, etc.)
    /// </summary>
    /// <returns>Returns user credential (like access token, refresh token, etc.)</returns>
    [HttpPost]
    public virtual async Task<IResult<LoginResponse<TUserKey>>> Login([FromBody] LoginRequest request, CancellationToken cancellationToken = default)
    {
        var authResult = await UserService.Authenticate(request.UserName, request.Password, cancellationToken);
        var response = Mapper.Map<LoginResponse<TUserKey>>(authResult);
        return response.ToResult();
    }

    [HttpPost]
    public virtual IResult<bool> Logout(CancellationToken cancellationToken = default)
    {
        // TODO: Implement revoke token
        return true.ToResult();
    }

    /// <summary>
    /// Refreshes user credential (like access token, refresh token, etc.)
    /// </summary>
    /// <returns>Returns refreshed user credential (like access token, refresh token, etc.)</returns>
    [HttpPost]
    public virtual async Task<IResult<LoginResponse<TUserKey>>> RefreshToken([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken = default)
    {
        var authResult = await UserService.RefreshToken(request.RefreshToken, request.Token, cancellationToken);
        var response = Mapper.Map<LoginResponse<TUserKey>>(authResult);
        return response.ToResult();
    }

    /// <summary>
    /// Sends a forgot password email to user
    /// </summary>
    /// <returns>If an exception is thrown, returns false, otherwise true</returns>
    [HttpPost]
    public virtual async Task<IResult<bool>> ForgotPassword([FromBody] ForgotPasswordRequest request, CancellationToken cancellationToken = default)
    {
        await UserService.ForgotPassword(request.UserName, cancellationToken);
        return true.ToResult();
    }

    /// <summary>
    /// Resets user password by token and new password
    /// </summary>
    /// <returns>If an exception is thrown, returns false, otherwise true</returns>
    [HttpPost]
    public virtual async Task<IResult<bool>> ResetPassword([FromBody] ResetPasswordRequest request, CancellationToken cancellationToken = default)
    {
        await UserService.ResetPassword(request.UserName, request.Token, request.NewPassword, cancellationToken);
        return true.ToResult();
    }

    /// <summary>
    /// Get info of current user
    /// </summary>
    /// <returns>Returns user info</returns>
    [HttpGet]
    [Authorize]
    public virtual async Task<IResult<UserResponse<TUserKey>>> GetCurrentUser(CancellationToken cancellationToken = default)
    {
        var userId = await UserService.GetCurrentUserId(cancellationToken);
        var user = await UserService.GetById(userId, cancellationToken); var userVm = Mapper.Map<UserResponse<TUserKey>>(user);
        return userVm.ToResult();
    }

    /// <summary>
    /// Get roles of current user
    /// </summary>
    /// <returns>Returns roles</returns>
    [HttpGet]
    [Authorize]
    public virtual async Task<IListResult<string>> GetCurrentRoles(CancellationToken cancellationToken = default)
    {
        var userId = await UserService.GetCurrentUserId(cancellationToken);
        var roles = await UserRoleService.GetRolesForUser(userId, cancellationToken);
        return roles.ToListResult();
    }

    /// <summary>
    /// Get claims of current user
    /// </summary>
    /// <returns>Returns claims</returns>
    [HttpGet]
    [Authorize]
    public virtual async Task<IListResult<Claim>> GetCurrentClaims(CancellationToken cancellationToken = default)
    {
        var userId = await UserService.GetCurrentUserId(cancellationToken);
        var user = await UserService.GetById(userId, cancellationToken);
        var claims = await UserService.GetClaims(user, cancellationToken);
        return claims.ToListResult();
    }

    /// <summary>
    /// Changes account info
    /// </summary>
    /// <returns>If an exception is thrown, returns false, otherwise true</returns>
    [HttpPost]
    [Authorize]
    public virtual async Task<IResult<bool>> Update([FromBody] UpdateAccountRequest request, CancellationToken cancellationToken = default)
    {
        var userId = await UserService.GetCurrentUserId(cancellationToken);
        var user = await UserService.GetById(userId, cancellationToken);
        Mapper.Map(request, user);
        await UserService.Update(user, cancellationToken);
        return true.ToResult();
    }

    /// <summary>
    /// Changes account password
    /// </summary>
    /// <returns>If an exception is thrown, returns false, otherwise true</returns>
    [HttpPost]
    [Authorize]
    public virtual async Task<IResult<bool>> ChangePassword([FromBody] ChangeAccountPasswordRequest request, CancellationToken cancellationToken = default)
    {
        var changePassword = Mapper.Map<ChangePassword<TUserKey>>(request);
        changePassword.UserId = await UserService.GetCurrentUserId(cancellationToken);
        await UserService.ChangePassword(changePassword, cancellationToken);
        return true.ToResult();
    }
}

/// <summary>
/// This class is an abstract controller (base controller) that has base action methods (endpoints) to accounts management.
/// </summary>
/// <typeparam name="TUser">Type of user entity -- TUser must have inherited from User</typeparam>
public abstract class AccountsControllerBase<TUser> : AccountsControllerBase<Guid, TUser>
    where TUser : User
{
    protected AccountsControllerBase(IUserService<TUser> userService, IUserRoleService<TUser> userRoleService, IMapper mapper) : base(userService, userRoleService, mapper)
    {
    }
}
