using System;
using Xunit;
using Core;
using Moq;

namespace Test
{
    public class CardTest
    {
        private string numberDummy = "1234-5678-8765-4321";
        private const int MIN_CVV = 100;
        private const int MAX_CVV = 999;
        private int cvvDummy = 777;
        private Mock<IDate> dateMocker = new Mock<IDate>();
        private IDate dateMock => dateMocker.Object;

        [Fact]
        public void IsInvalid_IfDateIsPassed()
        {
            dateMocker.SetupGet(x => x.IsPassed).Returns(true);
            var card = Card.Create(null, dateMock, cvvDummy, Currency.GBP);
            Assert.False(card.IsValid);
        }

        [Fact]
        public void IsInvalid_IfDateIsInvalid()
        {
            dateMocker.SetupGet(x => x.IsValid).Returns(false);
            var card = Card.Create(null, dateMock, cvvDummy, Currency.GBP);
            Assert.False(card.IsValid);
        }

        [Fact]
        public void IsInvalid_IfCvvBelowRange()
        {
            dateMocker.SetupGet(x => x.IsValid).Returns(true);
            var card = Card.Create(null, dateMock, MIN_CVV - 1, Currency.GBP);
            Assert.False(card.IsValid);
        }

        [Fact]
        public void IsInvalid_IfCvvExceedsRange()
        {
            dateMocker.SetupGet(x => x.IsValid).Returns(true);
            var card = Card.Create(null, dateMock, MAX_CVV + 1, Currency.GBP);
            Assert.False(card.IsValid);
        }

        [Fact(Skip = "todo")]
        public void Create_ValidatesCardNumber()
        {
        }

        [Fact(Skip = "todo")]
        public void Create_ValidatesCvv()
        {
        }
    }
}
