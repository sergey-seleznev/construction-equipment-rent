using ConstructionEquipmentRent.API.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("ConstructionEquipmentRent.API.Tests")]

namespace ConstructionEquipmentRent.API.Services
{
    public class OrderItemPriceCalculator : IOrderItemPriceCalculator
    {
        private readonly IStockRepository stockRepository;
        private readonly IRentalFeesProvider rentalFeesProvider;
        private readonly ILogger logger;
        
        public OrderItemPriceCalculator(
            IStockRepository stockRepository,
            IRentalFeesProvider rentalFeesProvider,
            ILogger<OrderItemPriceCalculator> logger)
        {
            this.stockRepository = stockRepository;
            this.rentalFeesProvider = rentalFeesProvider;
            this.logger = logger;
        }

        public virtual async Task<decimal?> Calculate(OrderItem orderItem)
        {
            var stockItem = await stockRepository.GetById(orderItem.StockItemId);
            if (stockItem == null)
            {
                logger.LogError($"Unable to calculate price for non-existing equipment: Id = {orderItem.StockItemId}!");
                return null;
            }

            var rentalFees = await rentalFeesProvider.Get();
            if (rentalFees == null)
            {
                logger.LogError("Unable to retrieve rental fees!");
                return null;
            }

            return Calculate(stockItem.Type, orderItem.DurationDays, rentalFees);
        }

        internal decimal? Calculate(string type, int days, RentalFees rentalFees)
        {
            if (days <= 0)
            {
                logger.LogError($"Unable to calculate price for negative day count: days = {days}!");
                return null;
            }

            switch (type)
            {
                case "Heavy":
                    return rentalFees.OneTimeFee +
                           days * rentalFees.PremiumDailyFee;

                case "Regular":
                    return rentalFees.OneTimeFee +
                           Math.Min(2, days) * rentalFees.PremiumDailyFee +
                           Math.Max(0, days - 2) * rentalFees.RegularDailyFee;

                case "Specialized":
                    return Math.Min(3, days) * rentalFees.PremiumDailyFee +
                           Math.Max(0, days - 3) * rentalFees.RegularDailyFee;

                default:
                    logger.LogError($"Unable to calculate price for unknown equipment type: {type}!");
                    return null;
            }
        }

    }
}
