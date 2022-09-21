using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Travellour.Business.Profiles;

public static class MapperServiceExtensions
{
    public static void AddMapperService(this IServiceCollection services)
    {
        services.AddScoped(provider => new MapperConfiguration(mc =>
        {
            mc.AddProfile(new Mapper());
        }).CreateMapper());
    }
}
