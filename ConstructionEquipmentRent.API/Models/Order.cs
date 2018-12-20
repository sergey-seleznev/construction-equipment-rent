using System.Collections.Generic;
using System.Linq;

namespace ConstructionEquipmentRent.API.Models
{
    public class Order
    {
        private List<OrderItem> items;

        public int Id { get; }

        public Order(int id)
        {
            Id = id;
            items = new List<OrderItem>();
        }

        public void AddItem(OrderItem item)
        {
            items.Add(item);

            items = items
                .OrderBy(i => i.StockItemId)
                .ThenByDescending(i => i.DurationDays)
                .ToList();
        }

        public IList<OrderItem> Items => items.ToList();

    }
}
