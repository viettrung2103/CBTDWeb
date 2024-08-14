using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    public class SD
    {
        //for User Role
        public const string AdminRole = "Admin";
        public const string ShipperRole = "Shipper";
        public const string CustomerRole = "Customer";

        //for Stripe Payment Status
        public const string PaymentStatusPending = "Payment Pending";
        public const string PaymentStatusApproved = "Payment Approved";

        //for Order Status        
        public const string StatusApproved = "Approved";
        public const string StatusInProcess = "Processing";
        public const string StatusShipped = "Shipped";
        public const string StatusCancelled = "Cancelled";
        public const string StatusRefunded = "Refunded";


    }
}
