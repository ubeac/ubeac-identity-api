using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using uBeac.Web;

namespace API;

/// <summary>
/// This class is an abstract controller (base controller) that has base action methods (endpoints) to users management.
/// </summary>
/// <typeparam name="TUserKey">Type of user entity key -- TKey must have inherited from IEquatable</typeparam>
/// <typeparam name="TUser">Type of user entity -- TUnit must have inherited from Unit</typeparam>
public abstract class UsersControllerBase<TUserKey, TUser> : BaseController
    where TUserKey : IEquatable<TUserKey>
    where TUser : User<TUserKey>
{
    protected readonly IUserService<TUserKey, TUser> UserService;
    protected readonly IUserRoleService<TUserKey, TUser> UserRoleService;
    protected readonly IMapper Mapper;

    protected UsersControllerBase(IUserService<TUserKey, TUser> userService, IUserRoleService<TUserKey, TUser> userRoleService, IMapper mapper)
    {
        UserService = userService;
        UserRoleService = userRoleService;
        Mapper = mapper;
    }

    /// <summary>
    /// Creates a new user
    /// </summary>
    /// <returns>Returns id of created user</returns>
    [HttpPost]
    public virtual async Task<IApiResult<TUserKey>> Create([FromBody] InsertUserRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var user = Mapper.Map<TUser>(request);
            await UserService.Insert(user, request.Password, cancellationToken);
            return user.Id.ToApiResult();
        }
        catch (Exception ex)
        {
            return ex.ToApiResult<TUserKey>();
        }
    }

    /// <summary>
    /// Updates a user
    /// </summary>
    /// <returns>If an exception is thrown, returns false, otherwise true</returns>
    [HttpPost]
    public virtual async Task<IApiResult<bool>> Update([FromBody] UpdateUserRequest<TUserKey> request, CancellationToken cancellationToken = default)
    {
        try
        {
            var user = await UserService.GetById(request.Id, cancellationToken);
            user.PhoneNumber = request.PhoneNumber;
            user.PhoneNumberConfirmed = request.PhoneNumberConfirmed;
            user.Email = request.Email;
            user.EmailConfirmed = request.EmailConfirmed;
            user.LockoutEnabled = request.LockoutEnabled;
            user.LockoutEnd = request.LockoutEnd;
            await UserService.Replace(user, cancellationToken);
            return true.ToApiResult();
        }
        catch (Exception ex)
        {
            return ex.ToApiResult<bool>();
        }
    }

    /// <summary>
    /// Assigns roles to user (Remove current roles and Add new roles)
    /// </summary>
    /// <returns>If an exception is thrown, returns false, otherwise true</returns>
    [HttpPost]
    public virtual async Task<IApiResult<bool>> AssignRoles([FromBody] AssignRoleRequest<TUserKey> request, CancellationToken cancellationToken = default)
    {
        try
        {
            // Remove current roles
            var currentRoles = await UserRoleService.GetRolesForUser(request.Id, cancellationToken);
            await UserRoleService.RemoveRoles(request.Id, currentRoles, cancellationToken);

            // Add new roles
            if (request.Roles?.Any() is true) await UserRoleService.AddRoles(request.Id, request.Roles, cancellationToken);

            return true.ToApiResult();
        }
        catch (Exception ex)
        {
            return ex.ToApiResult<bool>();
        }
    }

    /// <summary>
    /// Changes user password
    /// </summary>
    /// <returns>If an exception is thrown, returns false, otherwise true</returns>
    [HttpPost]
    public virtual async Task<IApiResult<bool>> ChangePassword([FromBody] ChangePasswordRequest<TUserKey> request, CancellationToken cancellationToken = default)
    {
        try
        {
            await UserService.ChangePassword(new ChangePassword<TUserKey>
            {
                UserId = request.UserId,
                CurrentPassword = request.CurrentPassword,
                NewPassword = request.NewPassword
            }, cancellationToken);
            return true.ToApiResult();
        }
        catch (Exception ex)
        {
            return ex.ToApiResult<bool>();
        }
    }

    /// <summary>
    /// Get user info by id
    /// </summary>
    /// <returns>Returns user info</returns>
    [HttpGet]
    public virtual async Task<IApiResult<UserResponse<TUserKey>>> GetById([FromQuery] IdRequest<TUserKey> request, CancellationToken cancellationToken = default)
    {
        try
        {
            var user = await UserService.GetById(request.Id, cancellationToken);
            var userVm = new UserResponse<TUserKey>
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                PhoneNumber = user.PhoneNumber,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed
            };
            return userVm.ToApiResult();
        }
        catch (Exception ex)
        {
            return ex.ToApiResult<UserResponse<TUserKey>>();
        }
    }

    /// <summary>
    /// Get all users
    /// </summary>
    /// <returns>Returns all units</returns>
    [HttpGet]
    public virtual async Task<IApiListResult<UserResponse<TUserKey>>> GetAll(CancellationToken cancellationToken = default)
    {
        try
        {
            var users = await UserService.GetAll(cancellationToken);
            var usersVm = users.Select(u => new UserResponse<TUserKey>
            {
                Id = u.Id,
                UserName = u.UserName,
                Email = u.Email,
                EmailConfirmed = u.EmailConfirmed,
                PhoneNumber = u.PhoneNumber,
                PhoneNumberConfirmed = u.PhoneNumberConfirmed
            });
            return usersVm.ToApiListResult();
        }
        catch (Exception ex)
        {
            return ex.ToApiListResult<UserResponse<TUserKey>>();
        }
    }
}

/// <summary>
/// This class is an abstract controller (base controller) that has base action methods (endpoints) to users management.
/// </summary>
/// <typeparam name="TUser">Type of user entity -- TUnit must have inherited from Unit</typeparam>
public abstract class UsersControllerBase<TUser> : UsersControllerBase<Guid, TUser>
    where TUser : User
{
    protected UsersControllerBase(IUserService<TUser> userService, IUserRoleService<TUser> userRoleService, IMapper mapper) : base(userService, userRoleService, mapper)
    {
    }
}