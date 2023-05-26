using AutoMapper;
using HueFestivalTicketOnline.Models.DTOs;
using HueFestivalTicketOnline.Models.Models;
namespace HueFestivalTicketOnline.Mapper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<ImageDTO, Image>().ForAllMembers(opts => 
                opts.Condition((src, dest, srcMember) => srcMember != null)); 
            CreateMap<FesProgramDTO, FesProgram>().ForAllMembers(opts =>
                opts.Condition((src, dest, srcMember) => srcMember != null)); 
            CreateMap<CreateLocationDTO, Location>().ForAllMembers(opts =>
                opts.Condition((src, dest, srcMember) => srcMember != null)); 
            CreateMap<SubMenuLocationDTO, SubMenuLocation>().ForAllMembers(opts =>
                opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<ViewSubMenuLocation, SubMenuLocation>().ReverseMap();
            CreateMap<ViewLocation, Location>().ReverseMap();
            CreateMap<ViewFesProgram, FesProgram>().ReverseMap(); 
            CreateMap<CreateDetailFesLocationDTO, DetailFesLocation>().ForAllMembers(opts =>
                opts.Condition((src, dest, srcMember) => srcMember != null)); 
            CreateMap<FesTypeTicketDTO, FesTypeTicket>().ForAllMembers(opts =>
                opts.Condition((src, dest, srcMember) => srcMember != null)); 
            CreateMap<CreateInvoiceTicketDTO, User>().ForAllMembers(opts =>
                opts.Condition((src, dest, srcMember) => srcMember != null)); 
            CreateMap<CreateNewsDTO, News>().ForAllMembers(opts =>
                opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<ChangeInfoAccount, Account>().ForAllMembers(opts =>
                opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<AboutInformationDTO, AboutInformation>().ForAllMembers(opts =>
                opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
