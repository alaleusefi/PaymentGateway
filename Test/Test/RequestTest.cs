using System;
using Xunit;
using Core;
using Moq;

namespace Test
{
    public class RequestTest
    {
        private const decimal MIN_AMOUNT = 0.5m;
        private const decimal MAX_AMOUNT = 500m;
        private int merchantIdDummy;
        private decimal amountDummy;
        private int requestIdDummy;
        private Mock<ICard> cardMocker = new Mock<ICard>();
        private ICard cardMock => cardMocker.Object;
        private Mock<IRepository<Request>> repoMocker = new Mock<IRepository<Request>>();
        private IRepository<Request> repoMock => repoMocker.Object;
        public RequestTest()
        {
            Random rng = new Random();
            merchantIdDummy = rng.Next(0, int.MaxValue - 200);
            requestIdDummy = rng.Next(0, int.MaxValue - 200);
            amountDummy = MIN_AMOUNT + new decimal(rng.NextDouble()) * (MAX_AMOUNT - MIN_AMOUNT);
            cardMocker.SetupGet(x => x.IsValid).Returns(true);
        }

        [Fact]
        public void Create_InitialisesState()
        {
            repoMocker.Setup(x => x.Save(It.IsAny<Request>())).Returns(requestIdDummy);
            var instance = Request.Create(merchantIdDummy, cardMock, amountDummy, repoMock);
            Assert.Equal(requestIdDummy, instance.Id);
            Assert.Equal(instance.MerchantId, merchantIdDummy);
            Assert.Equal(instance.Card, cardMock);
            Assert.Equal(instance.Amount, amountDummy);
            Assert.True(instance.IsValid);
        }

        [Fact]
        public void IsValid_IsFalse_IfMerchantIdNegative()
        {
            var instance = Request.Create(-1, cardMock, amountDummy, repoMock);
            Assert.False(instance.IsValid);
        }

        [Fact]
        public void IsValid_IsFalse_IfCardExpired()
        {
            cardMocker.SetupGet(x => x.IsValid).Returns(false);
            var instance = Request.Create(merchantIdDummy, cardMock, amountDummy, repoMock);
            Assert.False(instance.IsValid);
        }

        [Fact]
        public void IsValid_IsFalse_IfAmountBelowMinimum()
        {
            var instance = Request.Create(merchantIdDummy, cardMock, MIN_AMOUNT - 0.01m, repoMock);
            Assert.False(instance.IsValid);
        }

        [Fact]
        public void IsValid_IsFalse_IfAmountExceedsMaximum()
        {
            var instance = Request.Create(merchantIdDummy, cardMock, MAX_AMOUNT + 0.01m, repoMock);
            Assert.False(instance.IsValid);
        }

        [Fact]
        public void IsValid_IsTrue_Otherwise()
        {
            var instance = Request.Create(merchantIdDummy, cardMock, amountDummy, repoMock);
            Assert.True(instance.IsValid);
        }
    }
}