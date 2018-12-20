using ConstructionEquipmentRent.API.Models;
using ConstructionEquipmentRent.API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConstructionEquipmentRent.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IStockRepository stockRepository;

        public StockController(IStockRepository stockRepository)
        {
            this.stockRepository = stockRepository;
        }

        [HttpGet]
        public Task<IEnumerable<StockItem>> Get()
        {
            return stockRepository.GetAll();
        }
    }
}
