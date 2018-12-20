using System.Threading.Tasks;
using ConstructionEquipmentRent.API.Models;

namespace ConstructionEquipmentRent.API.Services
{
    public interface IOrderItemPriceCalculator
    {
        Task<decimal?> Calculate(OrderItem orderItem);
    }
}