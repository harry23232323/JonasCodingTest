using AutoMapper;
using BusinessLayer.Model.Models;
using DataAccessLayer.Model.Models;

namespace BusinessLayer
{
    public class BusinessProfile : Profile
    {
        public BusinessProfile()
        {
            CreateMapper();
        }

        private void CreateMapper()
        {
            CreateMap<DataEntity, BaseInfo>().ReverseMap();
            CreateMap<Company, CompanyInfo>().ReverseMap();
            CreateMap<ArSubledger, ArSubledgerInfo>().ReverseMap();

            CreateMap<Employee, EmployeeInfo>()
                .ForMember(dest => dest.OccupationName, opt => opt.MapFrom(src => src.Occupation))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Phone))
                .ForMember(dest => dest.LastModifiedDateTime,
                    opt => opt.MapFrom(src => src.LastModified.ToString("yyyy-MM-dd HH:mm:ss")))
                .ForMember(dest => dest.CompanyName, opt => opt.Ignore());

            CreateMap<EmployeeInfo, Employee>()
                .ForMember(dest => dest.Occupation, opt => opt.MapFrom(src => src.OccupationName))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.LastModified, opt => opt.Ignore());
        }
    }
}
