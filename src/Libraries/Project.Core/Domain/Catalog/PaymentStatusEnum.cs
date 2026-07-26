using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Core.Domain.Catalog;
public enum PaymentStatusEnum
{
    Captured = 1,    // Payment link created
    Pending = 2,    // Payment in progress
    Paid = 3,   // Payment successful
    Failed = 4,     // Payment failed
    Expired = 5,     // Payment link expired
    Refunded = 6,
    Partial = 7,
}
