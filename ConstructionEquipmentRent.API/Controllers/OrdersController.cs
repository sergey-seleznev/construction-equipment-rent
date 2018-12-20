using ConstructionEquipmentRent.API.Models;
using ConstructionEquipmentRent.API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace ConstructionEquipmentRent.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository orderRepository;
        private readonly IInvoiceGenerator invoiceGenerator;

        public OrdersController(
            IOrderRepository orderRepository,
            IInvoiceGenerator invoiceGenerator)
        {
            this.orderRepository = orderRepository;
            this.invoiceGenerator = invoiceGenerator;
        }
        
        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id)
        {
            var order = orderRepository.GetById(id);
            if (order == null)
                return NotFound();

            return Ok(order);
        }

        [HttpPost]
        public IActionResult Create()
        {
            var order = orderRepository.Create();
            if (order == null)
                return StatusCode((int)HttpStatusCode.InternalServerError);

            return Ok(order);
        }

        [HttpPost("{id}/items")]
        public IActionResult AddItems([FromRoute] int id,
            [FromBody] IEnumerable<OrderItem> orderItems)
        {
            var order = orderRepository.GetById(id);
            if (order == null)
                return NotFound();
            
            foreach (var orderItem in orderItems)
                order.AddItem(orderItem);
            
            return Ok(order.Items);
        }

        [HttpGet("{id}/invoice")]
        public async Task<IActionResult> GetInvoice([FromRoute] int id)
        {
            var order = orderRepository.GetById(id);
            if (order == null)
                return NotFound();

            var invoiceText = await invoiceGenerator.Generate(order);
            if (invoiceText == null)
                return StatusCode((int)HttpStatusCode.InternalServerError);
            
            return Ok(invoiceText);
        }
        
    }
}
