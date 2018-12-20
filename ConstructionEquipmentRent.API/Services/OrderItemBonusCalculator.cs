using System.Runtime.CompilerServices;
using ConstructionEquipmentRent.API.Models;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("ConstructionEquipmentRent.API.Tests")]

namespace ConstructionEquipmentRent.API.Services
{
    public class OrderItemBonusCalculator : IOrderItemBonusCalculator
    {
        private readonly IStockRepository stockRepository;
        private readonly ILogger logger;
        
        public OrderItemBonusCalculator(
            IStockRepository stockRepository,
            ILogger<OrderItemPriceCalculator> logger)
        {
            this.stockRepository = stockRepository;
            this.logger = logger;
        }

        public virtual async Task<int?> Calculate(OrderItem orderItem)
        {
            var stockItem = await stockRepository.GetById(orderItem.StockItemId);
            if (stockItem == null)
            {
                logger.LogError($"Unable to calculate bonus for non-existing equipment: Id = {orderItem.StockItemId}!");
                return null;
            }
            
            return Calculate(stockItem.Type);
        }

        internal int? Calculate(string type)
        {
            switch (type)
            {
                case "Heavy":
                    return 2;

                case "Regular":
                case "Specialized":
                    return 1;

                default:
                    logger.LogError($"Unable to calculate bonus for unknown equipment type: {type}!");
                    return null;
            }
        }

    }
}
