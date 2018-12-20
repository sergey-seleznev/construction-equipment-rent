using ConstructionEquipmentRent.API.Models;
using ConstructionEquipmentRent.API.Services;
using Moq;
using System.Linq;
using Xunit;

namespace ConstructionEquipmentRent.API.Tests
{
    public class OrderItemPriceCalculatorTests
    {
        [Theory, AutoMoqData]
        public void CalculateCorrectPrice(
            Mock<OrderItemPriceCalculator> sutMock)
        {
            var rentalFees = new RentalFees
            {
                OneTimeFee = 1,
                RegularDailyFee = 10,
                PremiumDailyFee = 100
            };

            var testCases = new[] 
            {
                new {type = "Heavy", days = 1, price = 101},        // 1 +  1x100
                new {type = "Heavy", days = 3, price = 301},        // 1 +  3x100
                new {type = "Heavy", days = 18, price = 1801},      // 1 + 18x100

                new {type = "Regular", days = 1, price = 101},      // 1 + 1x100
                new {type = "Regular", days = 2, price = 201},      // 1 + 2x100
                new {type = "Regular", days = 3, price = 211},      // 1 + 2x100 + 1x10
                new {type = "Regular", days = 7, price = 251},      // 1 + 2x100 + 5x10

                new {type = "Specialized", days = 1, price = 100},  // 1x100
                new {type = "Specialized", days = 3, price = 300},  // 3x100
                new {type = "Specialized", days = 4, price = 310},  // 3x100 +  1x10
                new {type = "Specialized", days = 28, price = 550}, // 3x100 + 25x10
            };

            testCases.ToList().ForEach(t =>
                Assert.Equal(t.price, sutMock.Object.Calculate(t.type, t.days, rentalFees)));
        }

        [Theory, AutoMoqData]
        public void DoesNotCalculateForNegativeDays(
            Mock<OrderItemPriceCalculator> sutMock,
            RentalFees rentalFees, int days)
        {
            Assert.Null(sutMock.Object.Calculate("Heavy", -1 * days, rentalFees));
        }

        [Theory, AutoMoqData]
        public void DoesNotCalculateForZeroDays(
            Mock<OrderItemPriceCalculator> sutMock,
            RentalFees rentalFees)
        {
            Assert.Null(sutMock.Object.Calculate("Heavy", 0, rentalFees));
        }

        [Theory, AutoMoqData]
        public void DoesNotCalculateForUnknownType(
            Mock<OrderItemPriceCalculator> sutMock,
            RentalFees rentalFees, string type, int days)
        {
            Assert.Null(sutMock.Object.Calculate(type, days, rentalFees));
        }
    }
}
