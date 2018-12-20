using System.Threading.Tasks;
using ConstructionEquipmentRent.API.Models;

namespace ConstructionEquipmentRent.API.Services
{
    public interface IOrderItemBonusCalculator
    {
        Task<int?> Calculate(OrderItem orderItem);
    }
}