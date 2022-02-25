using System;
using AutoMapper;

namespace API;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<InsertUserRequest, AppUser>();

        CreateMap<ChangeUserPasswordRequest, ChangePassword>();
        CreateMap<ChangeUserPasswordRequest<Guid>, ChangePassword<Guid>>();

        CreateMap<ChangeAccountPasswordRequest, ChangePassword>();
        CreateMap<ChangeAccountPasswordRequest, ChangePassword<Guid>>();

        CreateMap<AppUser, UserResponse>();
        CreateMap<AppUser, UserResponse<Guid>>();
    }
}