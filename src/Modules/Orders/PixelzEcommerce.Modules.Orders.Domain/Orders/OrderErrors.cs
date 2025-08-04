using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PixelzEcommerce.Shared.Domain;

namespace PixelzEcommerce.Modules.Orders.Domain.Orders;
public static class OrderErrors
{
    public static readonly Error OrderNotCheckedOut = Error.Problem(
        "Orders.MustCheckedOut",
        "Order must be checked out to mark as paid.");

    public static readonly Error OrderWasCheckout = Error.Conflict(
        "Orders.OrderWasCheckout",
        "Order was already checked out and cannot be checked out again."
        );
}
