using Microsoft.Extensions.DependencyInjection;
using NTier.BLL.AutoMapper;
using NTier.BLL.MappingImplementation;
using NTier.BLL.MappingInterface;
using NTier.BLL.ServiceImplementation;
using NTier.BLL.ServiceInterface;

namespace NTier.BLL.DI;

public static class ServiceContainer
{
    public static IServiceCollection AddBLLServices(this IServiceCollection services)
    {
        // Register generic mapper - uses reflection-based property mapping by default
        services.AddScoped(typeof(IMapper<,>), typeof(Mapper<,>));

        // Register services
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<IDepartmentService, DepartmentService>();

        // Register AutoMapper profiles for employee and department mappings
        services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<EmployeeMappingProfile>();
            cfg.AddProfile<DepartmentMappingProfile>();
        });

        return services;
    }
}