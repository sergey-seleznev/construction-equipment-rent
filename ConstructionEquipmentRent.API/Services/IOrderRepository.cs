using ConstructionEquipmentRent.API.Models;

namespace ConstructionEquipmentRent.API.Services
{
    public interface IOrderRepository
    {
        Order Create();
        Order GetById(int id);
    }
}
