using AutoMapper;
using Domain.Dto;
using Domain.Models;


public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<UserRegisterDto, User>();
        
    }
}
