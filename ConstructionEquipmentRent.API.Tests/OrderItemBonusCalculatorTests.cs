using ConstructionEquipmentRent.API.Models;
using ConstructionEquipmentRent.API.Services;
using Moq;
using System.Linq;
using Xunit;

namespace ConstructionEquipmentRent.API.Tests
{
    public class OrderItemBonusCalculatorTests
    {
        [Theory, AutoMoqData]
        public void CalculateCorrectBonus(
            Mock<OrderItemBonusCalculator> sutMock)
        {
            var testCases = new[] 
            {
                new {type = "Heavy", bonus = 2},
                new {type = "Regular", bonus = 1},
                new {type = "Specialized", bonus = 1},
            };

            testCases.ToList().ForEach(t =>
                Assert.Equal(t.bonus, sutMock.Object.Calculate(t.type)));
        }
        
        [Theory, AutoMoqData]
        public void DoesNotCalculateForUnknownType(
            Mock<OrderItemBonusCalculator> sutMock,
            string type)
        {
            Assert.Null(sutMock.Object.Calculate(type));
        }
    }
}
