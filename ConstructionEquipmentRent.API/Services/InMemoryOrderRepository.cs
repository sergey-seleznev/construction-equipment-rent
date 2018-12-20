using ConstructionEquipmentRent.API.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Threading;

namespace ConstructionEquipmentRent.API.Services
{
    public class InMemoryOrderRepository : IOrderRepository
    {
        private readonly ILogger logger;

        private readonly ConcurrentDictionary<int, Order> orders;
        private int orderIdCounter;

        public InMemoryOrderRepository(ILogger<InMemoryOrderRepository> logger)
        {
            this.logger = logger;

            orders = new ConcurrentDictionary<int, Order>();
            orderIdCounter = 0;
        }

        public Order Create()
        {
            var orderId = Interlocked.Increment(ref orderIdCounter);
            var order = new Order(orderId);

            orders[orderId] = order;

            return order;
        }

        public Order GetById(int id)
        {
            if (!orders.TryGetValue(id, out var order))
            {
                logger.LogWarning($"Requested order not found: Id = {id}");
                return null;
            }

            return order;
        }
    }
}
