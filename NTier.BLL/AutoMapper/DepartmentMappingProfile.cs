using AutoMapper;
using NTier.BLL.ViewModels.DepartmentViewModels;
using NTier.DAL.Models;

namespace NTier.BLL.AutoMapper;

public class DepartmentMappingProfile : Profile
{
    public DepartmentMappingProfile()
    {
        CreateMap<Department, CreateDepartmentVM>().ReverseMap();
        CreateMap<Department, EditDepartmentVM>().ReverseMap();
        CreateProjection<Department, DepartmentVM>();
        CreateProjection<DepartmentVM, Department>();
    }
}