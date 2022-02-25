using System;
using AutoMapper;

namespace API;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<InsertUserRequest, AppUser>();

        CreateMap<ChangePasswordRequest, ChangePassword>();
        CreateMap<ChangePasswordRequest<Guid>, ChangePassword<Guid>>();

        CreateMap<AppUser, UserResponse>();
        CreateMap<AppUser, UserResponse<Guid>>();
    }
}