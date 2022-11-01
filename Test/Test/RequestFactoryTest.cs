using System;
using Xunit;
using Core;
using Moq;

namespace Test
{
    public class RequestFactoryTest
    {
        private readonly IRepository<Request> repo;
        private readonly RequestFactory instance;
        public RequestFactoryTest()
        {
            repo = new FakeRepo<Request>();
            instance = new RequestFactory(repo);
        }
        private readonly Dto dto = new Dto
        {
            MerchantId = 1,
            Amount = 0.7m,
            Card_Number = "1234-5678-8765-4321",
            Card_Cvv = 777,
            Card_Currency = Currency.GBP,
            Card_Expiry_Year = 2023,
            Card_Expiry_Month = 5
        };

        [Fact]
        public void Make_Makes()
        {
            var result = instance.Make(dto);
            Assert.IsType<Request>(result);
        }
    }
}