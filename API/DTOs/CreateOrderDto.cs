using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.OrderAggregate;

namespace API.DTOs
{
    public class CreateOrderDto
    {
        [Required]
        public string CartID { get; set; } = string.Empty;
        [Required]
        public int DeliveryMethodID { get; set; }
        [Required]
        public ShippingAddress ShippingAddress { get; set; } = null!;
        [Required]
        public PaymentSummary PaymentSummary { get; set; } = null!;

    }
}