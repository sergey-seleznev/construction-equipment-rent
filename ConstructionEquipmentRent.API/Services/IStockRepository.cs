using ConstructionEquipmentRent.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConstructionEquipmentRent.API.Services
{
    public interface IStockRepository
    {
        Task<IEnumerable<StockItem>> GetAll();

        Task<StockItem> GetById(int id);
    }
}
