using ConstructionEquipmentRent.API.Models;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Threading.Tasks;

namespace ConstructionEquipmentRent.API.Services
{
    public class InvoiceGenerator : IInvoiceGenerator
    {
        private readonly IStockRepository stockRepository;
        private readonly IOrderItemPriceCalculator orderItemPriceCalculator;
        private readonly IOrderItemBonusCalculator orderItemBonusCalculator;
        private readonly ILogger logger;
        
        public InvoiceGenerator(
            IStockRepository stockRepository,
            IOrderItemPriceCalculator orderItemPriceCalculator,
            IOrderItemBonusCalculator orderItemBonusCalculator,
            ILogger<InvoiceGenerator> logger)
        {
            this.stockRepository = stockRepository;
            this.orderItemPriceCalculator = orderItemPriceCalculator;
            this.orderItemBonusCalculator = orderItemBonusCalculator;
            this.logger = logger;
        }
        
        public async Task<string> Generate(Order order)
        {
            var sb = new StringBuilder();

            var totalPrice = 0m;
            var totalBonus = 0;

            sb.AppendFormat("INVOICE #{0}\n\n", order.Id);

            foreach (var orderItem in order.Items)
            {
                var stockItem = await stockRepository.GetById(orderItem.StockItemId);
                if (stockItem == null)
                {
                    logger.LogError($"Unable to generate invoice for order containing unknown stock item: Id = {orderItem.StockItemId}!");
                    return null;
                }

                var price = await orderItemPriceCalculator.Calculate(orderItem);
                if (price == null)
                {
                    logger.LogError("Unable to generate invoice for order containing unknown price item!");
                    return null;
                }

                var bonus = await orderItemBonusCalculator.Calculate(orderItem);
                if (bonus == null)
                {
                    logger.LogError("Unable to generate invoice for order containing unknown bonus item!");
                    return null;
                }

                totalPrice += price.Value;
                totalBonus += bonus.Value;
                
                sb.AppendFormat("{0}\t\t{1}€\n", stockItem.Name, price);
            }

            sb.AppendFormat("\nTOTAL\t\t{0}€\n", totalPrice);
            sb.AppendFormat("BONUS\t\t{0}pt.", totalBonus);

            return sb.ToString();
        }

    }
}
