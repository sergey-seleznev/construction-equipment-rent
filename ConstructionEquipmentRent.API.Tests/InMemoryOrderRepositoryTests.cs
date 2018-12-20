using ConstructionEquipmentRent.API.Models;
using ConstructionEquipmentRent.API.Services;
using System.Collections.Generic;
using Xunit;

namespace ConstructionEquipmentRent.API.Tests
{
    public class InMemoryOrderRepositoryTests
    {
        [Theory, AutoMoqData]
        public void StoresCreatedOrders(InMemoryOrderRepository sut, int orderCount)
        {
            // random number 2..10
            orderCount = orderCount % 9 + 2;
            
            var createdOrders = new List<Order>();
            for (var i = 0; i < orderCount; i++)
                createdOrders.Add(sut.Create());

            for (var i = 0; i < orderCount; i++)
            {
                var createdOrder = createdOrders[i];
                var order = sut.GetById(createdOrder.Id);

                Assert.Equal(createdOrder, order);
                Assert.Equal(i + 1, order.Id);
            }
        }

    }
}
