using AutoMapper;
using CandidateWebApplication.Models;
using Microsoft.AspNetCore.Identity;

namespace CandidateWebApplication.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<IdentityUser, Candidates>()
                       .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id));

        }
    }
}
