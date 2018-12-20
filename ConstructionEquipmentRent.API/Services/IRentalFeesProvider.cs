using ConstructionEquipmentRent.API.Models;
using System.Threading.Tasks;

namespace ConstructionEquipmentRent.API.Services
{
    public interface IRentalFeesProvider
    {
        Task<RentalFees> Get();
    }
}
