using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class ShoppingCart
    {
        public string ID { get; set; }
        public List<CartItem> Items { get; set; } =[];
    }
}