using System.Data;
using Bogus;
using Dapper;
using PixelzEcommerce.Modules.Orders.Domain.Orders;
using PixelzEcommerce.Shared.Application.Data;

namespace PixelzEcommerce.Api.Extensions;

internal static class SeedDataExtensions
{
    public static void SeedData(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        IDbConnectionFactory sqlConnectionFactory = scope.ServiceProvider.GetRequiredService<IDbConnectionFactory>();
        using IDbConnection connection = sqlConnectionFactory.CreateConnection();

        Guid? orderId = connection.QuerySingleOrDefault<Guid?>(
            @"SELECT id FROM ""order"".orders LIMIT 1;"
        );

        if (orderId == null)
        {
            Faker<Order> orderFaker = new Faker<Order>()
               .CustomInstantiator(f => Order.Create(
                   name: f.Commerce.ProductName(),
                   status: f.PickRandom<OrderStatus>(),
                   totalAmount: f.Random.Number(1,999),
                   createdAt: f.Date.PastOffset(3).DateTime
               ));

            List<Order> orders = orderFaker.Generate(50);

            connection.Execute(
                "INSERT INTO \"order\".\"orders\" (id, name, status, total_amount, created_at) VALUES (@Id, @Name, @Status, @TotalAmount, @CreatedAt)",
                orders.Select(o => new
                {
                    o.Id,
                    o.Name,
                    Status = (int)o.Status,
                    o.TotalAmount,
                    o.CreatedAt
                })
            );
        }
    }
}
