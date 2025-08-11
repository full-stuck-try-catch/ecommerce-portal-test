using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using PixelzEcommerce.Modules.Orders.Application.Abstractions.Data;
using PixelzEcommerce.Modules.Orders.Application.Orders.CheckoutOrder;
using PixelzEcommerce.Modules.Orders.Domain.Orders;
using PixelzEcommerce.Modules.Orders.Infrastructure.Database;
using PixelzEcommerce.Modules.Orders.Infrastructure.DbInterceptors;
using PixelzEcommerce.Modules.Orders.Infrastructure.Repositories;
using PixelzEcommerce.Modules.Products.Presentation.Products;
using PixelzEcommerce.Shared.Infrastructure.Outbox;
using PixelzEcommerce.Shared.Presentation.Endpoints;

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
                .UseSnakeCaseNamingConvention()
                .AddInterceptors(sp.GetRequiredService<InsertOutboxMessagesInterceptor>()));

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<OrderDbContext>());
        services.AddScoped<IOrderRepository, OrderRepository>();

    }
}
