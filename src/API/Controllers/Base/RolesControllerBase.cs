using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using uBeac.Web;

namespace API;

/// <summary>
/// This class is an abstract controller (base controller) that has base action methods (endpoints) to roles management.
/// </summary>
/// <typeparam name="TRoleKey">Type of role entity key -- TKey must have inherited from IEquatable</typeparam>
/// <typeparam name="TRole">Type of role entity -- TRole must have inherited from Role</typeparam>
[Authorize(Roles = "ADMIN")]
public abstract class RolesControllerBase<TRoleKey, TRole> : BaseController
   where TRoleKey : IEquatable<TRoleKey>
   where TRole : Role<TRoleKey>
{
    protected readonly IRoleService<TRoleKey, TRole> RoleService;
    protected readonly IMapper Mapper;

    protected RolesControllerBase(IRoleService<TRoleKey, TRole> roleService, IMapper mapper)
    {
        RoleService = roleService;
        Mapper = mapper;
    }

    /// <summary>
    /// Creates a new role
    /// </summary>
    /// <returns>If an exception is thrown, returns false, otherwise true</returns>
    [HttpPost]
    public virtual async Task<IResult<TRoleKey>> Create([FromBody] CreateRoleRequest request, CancellationToken cancellationToken = default)
    {
        var role = Mapper.Map<TRole>(request);
        await RoleService.Create(role, cancellationToken);
        return role.Id.ToResult();
    }

    /// <summary>
    /// Updates a role
    /// </summary>
    /// <returns>If an exception is thrown, returns false, otherwise true</returns>
    [HttpPost]
    public virtual async Task<IResult<bool>> Update([FromBody] UpdateRoleRequest<TRoleKey> request, CancellationToken cancellationToken = default)
    {
        var role = await RoleService.GetById(request.Id, cancellationToken);
        Mapper.Map(request, role);
        await RoleService.Update(role, cancellationToken);
        return true.ToResult();
    }

    /// <summary>
    /// Deletes a role
    /// </summary>
    /// <returns>If an exception is thrown, returns false, otherwise true</returns>
    [HttpPost]
    public virtual async Task<IResult<bool>> Delete([FromBody] IdRequest<TRoleKey> request, CancellationToken cancellationToken = default)
    {
        await RoleService.Delete(request.Id, cancellationToken);
        return true.ToResult();
    }

    /// <summary>
    /// Get all roles
    /// </summary>
    /// <returns>Returns all roles</returns>
    [HttpGet]
    public virtual async Task<IListResult<TRole>> GetAll(CancellationToken cancellationToken = default)
    {
        var roles = await RoleService.GetAll(cancellationToken);
        return roles.ToListResult();
    }

    /// <summary>
    /// Get role info by id
    /// </summary>
    /// <returns>Returns role info</returns>
    [HttpGet]
    public virtual async Task<IResult<TRole>> GetById([FromQuery] IdRequest<TRoleKey> request, CancellationToken cancellationToken = default)
    {
        var role = await RoleService.GetById(request.Id, cancellationToken);
        return role.ToResult();
    }
}

/// <summary>
/// This class is an abstract controller (base controller) that has base action methods (endpoints) to roles management.
/// </summary>
/// <typeparam name="TRole">Type of role entity -- TRole must have inherited from Role</typeparam>
public abstract class RolesControllerBase<TRole> : RolesControllerBase<Guid, TRole>
   where TRole : Role
{
    protected RolesControllerBase(IRoleService<TRole> roleService, IMapper mapper) : base(roleService, mapper)
    {
    }
}