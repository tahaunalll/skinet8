using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Core.Entities.OrderAggregate;

namespace Core.Specifications
{
    public class OrderSpecification: BaseSpecification<Order>
    {
        public OrderSpecification(string email) : base(x=>x.BuyerEmail == email)
        {
            AddInclude(x=>x.OrderItems);
            AddInclude(x=>x.DeliveryMethod);
            AddOrderByDescending(x=>x.OrderDate);
        }

        public OrderSpecification(string email, int id) : base(x => x.BuyerEmail ==email && x.ID ==id)
        {
            AddInclude("OrderItems");
            AddInclude("DeliveryMethod");
        }

        public OrderSpecification(string paymentIntentId, bool isPaymentIntent):
        base(x=>x.PaymentIntentID == paymentIntentId)
        {
             AddInclude("OrderItems");
            AddInclude("DeliveryMethod");   
        }
        
    }
}