using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConstructionEquipmentRent.API.Models
{
    public class OrderItem
    {
        public int StockItemId { get; set; }

        public int DurationDays { get; set; }
    }
}
