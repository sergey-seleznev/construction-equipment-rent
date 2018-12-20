using ConstructionEquipmentRent.API.Models;
using System.Threading.Tasks;

namespace ConstructionEquipmentRent.API.Services
{
    public interface IInvoiceGenerator
    {
        Task<string> Generate(Order order);
    }
}