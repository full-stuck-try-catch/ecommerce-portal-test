using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PixelzEcommerce.Shared.Domain;

namespace PixelzEcommerce.Modules.Orders.Domain.Orders;
public static class OrderErrors
{
    public static readonly Error OrderAlreadyCheckedOut = Error.Conflict(
        "Orders.OrderAlreadyCheckedOut",
        "Order was already checked out and cannot be checked out again."
        );

    public static readonly Error OrderNotFound = Error.NotFound("Order.NotFound", "Order was not found");
}
