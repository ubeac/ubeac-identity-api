using System;
using AutoMapper;

namespace API;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateUserRequest, AppUser>();
        CreateMap<RegisterRequest, AppUser>();

        CreateMap<UpdateAccountRequest, AppUser>();

        CreateMap<AppRole, AppRole>();

        CreateMap<ChangeUserPasswordRequest, ChangePassword>();
        CreateMap<ChangeUserPasswordRequest<Guid>, ChangePassword<Guid>>();

        CreateMap<UpdateUserRequest, AppUser>();
        CreateMap<UpdateUserRequest<Guid>, AppUser>();

        CreateMap<ChangeAccountPasswordRequest, ChangePassword>();
        CreateMap<ChangeAccountPasswordRequest, ChangePassword<Guid>>();

        CreateMap<AppUser, UserResponse>();
        CreateMap<AppUser, UserResponse<Guid>>();

        CreateMap<SignInResult, LoginResponse>();
        CreateMap<SignInResult<Guid>, LoginResponse<Guid>>();

        CreateMap<TokenResult, RegisterResponse>();
        CreateMap<SignInResult<Guid>, RegisterResponse<Guid>>();
    }
}