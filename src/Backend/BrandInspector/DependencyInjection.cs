using BrandInspector.Data;
using BrandInspector.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;

namespace BrandInspector;

public static class DependencyInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {


        return services;
    }

    public async static Task<WebApplication> UseServices(this WebApplication app)
    {


        return app;
    }





}
