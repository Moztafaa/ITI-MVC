using AutoMapper;
using NTier.BLL.ViewModels;
using NTier.DAL.Models;

namespace NTier.BLL.AutoMapper;

public class EmployeeMappingProfile : Profile
{
    public EmployeeMappingProfile()
    {
        CreateMap<Employee, CreateEmployeeVM>().ReverseMap();
        CreateMap<Employee, EmployeeVM>().ReverseMap();
        CreateMap<Employee, EditEmployeeVM>().ReverseMap();
    }
}