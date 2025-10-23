using Microsoft.Extensions.DependencyInjection;
using NTier.DAL.DataBaseContext;
using NTier.DAL.RepositoryImplementation;
using NTier.DAL.RepositoryInterface;

namespace NTier.DAL.DI;

public static class ServiceContainer
{
    public static IServiceCollection AddDALService(this IServiceCollection services)
    {
        services.AddScoped<IEmployeeRepo, EmployeeRepo>();
        return services;
    }
}