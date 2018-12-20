using AutoFixture.Xunit2;
using ConstructionEquipmentRent.API.Models;
using ConstructionEquipmentRent.API.Services;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ConstructionEquipmentRent.API.Tests
{
    public class InvoiceGeneratorTests
    {
        [Theory, AutoMoqData]
        public async Task InvoiceContainsTitle(
            Mock<InvoiceGenerator> sutMock,
            Order order)
        {
            var invoice = await sutMock.Object.Generate(order);
            Assert.Contains($"INVOICE #{order.Id}\n", invoice);
        }

        [Theory, AutoMoqData]
        public async Task InvoiceContainsItems(
            [Frozen] Mock<IStockRepository> stockRepositoryMock,
            [Frozen] Mock<IOrderItemPriceCalculator> orderItemPriceCalculatorMock,
            [Frozen] Mock<IOrderItemBonusCalculator> orderItemBonusCalculatorMock,
            InvoiceGenerator sut, Order order, List<OrderItem> orderItems)
        {
            orderItems.ForEach(order.AddItem);

            // mock name = "stock-item-{Id}"
            stockRepositoryMock.Setup(m => m.GetById(It.IsAny<int>()))
                .Returns<int>(id => Task.FromResult(new StockItem {Name = $"stock-item-{id}"}));

            // mock price = {StockItemId} x {DurationDays}
            orderItemPriceCalculatorMock.Setup(m => m.Calculate(It.IsAny<OrderItem>()))
                .Returns<OrderItem>(i => Task.FromResult((decimal?)(i.StockItemId * i.DurationDays)));

            // mock bonus = 0
            orderItemBonusCalculatorMock.Setup(m => m.Calculate(It.IsAny<OrderItem>()))
                .ReturnsAsync(0);

            var invoice = await sut.Generate(order);

            orderItems.ForEach(i =>
                Assert.Contains($"\nstock-item-{i.StockItemId}\t\t{i.StockItemId * i.DurationDays}€\n", invoice));            
        }

        [Theory, AutoMoqData]
        public async Task InvoiceContainsTotalPrice(
            [Frozen] Mock<IStockRepository> stockRepositoryMock,
            [Frozen] Mock<IOrderItemPriceCalculator> orderItemPriceCalculatorMock,
            [Frozen] Mock<IOrderItemBonusCalculator> orderItemBonusCalculatorMock,
            InvoiceGenerator sut, Order order, List<OrderItem> orderItems)
        {
            orderItems.ForEach(order.AddItem);
            
            stockRepositoryMock.Setup(m => m.GetById(It.IsAny<int>()))
                .Returns<int>(id => Task.FromResult(new StockItem()));

            // mock price = {StockItemId} x {DurationDays}
            orderItemPriceCalculatorMock.Setup(m => m.Calculate(It.IsAny<OrderItem>()))
                .Returns<OrderItem>(i => Task.FromResult((decimal?)(i.StockItemId * i.DurationDays)));

            // mock bonus = 0
            orderItemBonusCalculatorMock.Setup(m => m.Calculate(It.IsAny<OrderItem>()))
                .ReturnsAsync(0);

            var invoice = await sut.Generate(order);
            var expectedTotalPrice = orderItems.Sum(i => i.StockItemId * i.DurationDays);

            Assert.Contains($"\nTOTAL\t\t{expectedTotalPrice}€\n", invoice);
        }

        [Theory, AutoMoqData]
        public async Task InvoiceContainsTotalBonus(
            [Frozen] Mock<IStockRepository> stockRepositoryMock,
            [Frozen] Mock<IOrderItemPriceCalculator> orderItemPriceCalculatorMock,
            [Frozen] Mock<IOrderItemBonusCalculator> orderItemBonusCalculatorMock,
            InvoiceGenerator sut, Order order, List<OrderItem> orderItems)
        {
            orderItems.ForEach(order.AddItem);
            
            stockRepositoryMock.Setup(m => m.GetById(It.IsAny<int>()))
                .Returns<int>(id => Task.FromResult(new StockItem()));

            // mock price = 0
            orderItemPriceCalculatorMock.Setup(m => m.Calculate(It.IsAny<OrderItem>()))
                .ReturnsAsync(0);

            // mock bonus = {StockItemId} x {DurationDays}
            orderItemBonusCalculatorMock.Setup(m => m.Calculate(It.IsAny<OrderItem>()))
                .Returns<OrderItem>(i => Task.FromResult((int?)(i.StockItemId * i.DurationDays)));

            var invoice = await sut.Generate(order);
            var expectedTotalBonus = orderItems.Sum(i => i.StockItemId * i.DurationDays);

            Assert.Contains($"\nBONUS\t\t{expectedTotalBonus}pt.", invoice);
        }
        
    }
}
