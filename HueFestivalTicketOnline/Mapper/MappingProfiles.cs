using AutoMapper;
using HueFestivalTicketOnline.Models.DTOs;
using HueFestivalTicketOnline.Models.Models;
namespace HueFestivalTicketOnline.Mapper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<ImageDTO, Image>().ReverseMap();
            CreateMap<FesProgramDTO, FesProgram>().ReverseMap();
            CreateMap<CreateLocationDTO, Location>().ReverseMap();  
            CreateMap<SubMenuLocationDTO, SubMenuLocation>().ReverseMap();
            CreateMap<ViewSubMenuLocation, SubMenuLocation>().ReverseMap();
            CreateMap<ViewLocation, Location>().ReverseMap();
            CreateMap<ViewFesProgram, FesProgram>().ReverseMap();
            CreateMap<DetailFesLocation, CreateDetailFesLocationDTO>().ReverseMap();
            CreateMap<FesTypeTicketDTO, FesTypeTicket>();
            CreateMap<CreateInvoiceTicketDTO, User>();
            CreateMap<CreateNewsDTO, News>().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
