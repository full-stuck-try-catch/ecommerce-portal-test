using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PixelzEcommerce.Modules.Orders.Application.Abstractions.Data;
using PixelzEcommerce.Modules.Orders.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using PixelzEcommerce.Modules.Products.Presentation.Products;
using PixelzEcommerce.Shared.Presentation.Endpoints;
using MassTransit;

namespace PixelzEcommerce.Modules.Orders.Infrastructure;
public static class OrderModule
{
    public static IServiceCollection AddOrderModule(
       this IServiceCollection services,
       IConfiguration configuration)
    {

        services.AddInfrastructure(configuration);
        services.AddEndpoints(Presentation.AssemblyReference.Assembly);
        return services;
    }


    public static void ConfigureConsumers(IRegistrationConfigurator registrationConfigurator)
    {
        registrationConfigurator.AddConsumer<ProductOrderedIntegrationConsumer>();
    }

    private static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<OrderDbContext>((sp, options) =>
            options
                .UseNpgsql(
                    configuration.GetConnectionString("Database"),
                    npgsqlOptions => npgsqlOptions
                        .MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Order))
                .UseSnakeCaseNamingConvention());

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<OrderDbContext>());
    }
}
