using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using uBeac.Web;

namespace Example;

public class UsersController : UsersControllerBase<CustomUser>
{
    public UsersController(IUserService<CustomUser> userService) : base(userService)
    {
    }
}

public abstract class UsersControllerBase<TUserKey, TUser> : BaseController
    where TUserKey : IEquatable<TUserKey>
    where TUser : User<TUserKey>
{
    protected readonly IUserService<TUserKey, TUser> UserService;

    protected UsersControllerBase(IUserService<TUserKey, TUser> userService)
    {
        UserService = userService;
    }

    [HttpPost]
    public virtual async Task<IApiResult<TUserKey>> Insert([FromBody] InsertUserRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var user = Activator.CreateInstance<TUser>();
            user.UserName = request.UserName;
            user.PhoneNumber = request.PhoneNumber;
            user.PhoneNumberConfirmed = request.PhoneNumberConfirmed;
            user.Email = request.Email;
            user.EmailConfirmed = request.EmailConfirmed;
            await UserService.Insert(user, request.Password, cancellationToken);
            return user.Id.ToApiResult();
        }
        catch (Exception ex)
        {
            return ex.ToApiResult<TUserKey>();
        }
    }

    [HttpPost]
    public virtual async Task<IApiResult<bool>> Update([FromBody] ReplaceUserRequest<TUserKey> request, CancellationToken cancellationToken = default)
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

    [HttpGet]
    public virtual async Task<IApiResult<UserViewModel<TUserKey>>> Find([FromQuery] IdRequest<TUserKey> request, CancellationToken cancellationToken = default)
    {
        try
        {
            var user = await UserService.GetById(request.Id, cancellationToken);
            var userVm = new UserViewModel<TUserKey>
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
            return ex.ToApiResult<UserViewModel<TUserKey>>();
        }
    }

    [HttpGet]
    public virtual async Task<IApiListResult<UserViewModel<TUserKey>>> All(CancellationToken cancellationToken = default)
    {
        try
        {
            var users = await UserService.GetAll(cancellationToken);
            var usersVm = users.Select(u => new UserViewModel<TUserKey>
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
            return ex.ToApiListResult<UserViewModel<TUserKey>>();
        }
    }
}

public abstract class UsersControllerBase<TUser> : UsersControllerBase<Guid, TUser>
    where TUser : User
{
    protected UsersControllerBase(IUserService<TUser> userService) : base(userService)
    {
    }
}