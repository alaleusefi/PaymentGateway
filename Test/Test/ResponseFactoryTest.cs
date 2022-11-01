using System;
using Xunit;
using Core;
using Moq;

namespace Test
{
    public class ResponseFactoryTest
    {
        private readonly ResponseFactory instance;
        private readonly Mock<IRequestable> requestMocker = new Mock<IRequestable>();
        private IRequestable requestMock => requestMocker.Object;
        private readonly Mock<IBank> bankMocker = new Mock<IBank>();
        private IBank bankMock => bankMocker.Object;

        public ResponseFactoryTest()
        {
            instance = new ResponseFactory(bankMock);
        }

        [Fact]
        public void Process_OnlyAcceptsFreshRequests()
        {
            requestMocker.SetupGet(x => x.IsSuccess).Returns(true);
            Assert.Throws<ArgumentException>(
                () => instance.Process(requestMock)
            );
            requestMocker.SetupGet(x => x.IsSuccess).Returns(false);
            Assert.Throws<ArgumentException>(
                () => instance.Process(requestMock)
            );
        }

        [Fact]
        public void Process_ReturnsInvalidStatus_IfRequestIsInvalid()
        {
            requestMocker.SetupGet(x => x.IsValid).Returns(false);
            var response = instance.Process(requestMock);
            Assert.Equal(Status.Invalid, response.Status);
        }

        [Fact]
        public void Process_ReturnsSuccessStatus_IfBankAcceptsRequest()
        {
            requestMocker.SetupGet(x => x.IsValid).Returns(true);
            bankMocker.Setup(x => x.Pay(It.IsAny<IRequestable>())).Returns(true);
            var response = instance.Process(requestMock);
            Assert.Equal(Status.Success, response.Status);
        }

        [Fact]
        public void Process_SetsRequestSuccess_IfBankAcceptsRequest()
        {
            requestMocker.SetupGet(x => x.IsValid).Returns(true);
            requestMocker.SetupSet(x => x.IsSuccess = It.IsAny<bool?>()).Verifiable();
            bankMocker.Setup(x => x.Pay(It.IsAny<IRequestable>())).Returns(true);
            instance.Process(requestMock);
            requestMocker.VerifySet(x => x.IsSuccess = true);
        }

        [Fact]
        public void Process_ClearsRequestSuccess_IfBankDeclinesRequest()
        {
            requestMocker.SetupGet(x => x.IsValid).Returns(true);
            requestMocker.SetupSet(x => x.IsSuccess = It.IsAny<bool?>()).Verifiable();
            bankMocker.Setup(x => x.Pay(It.IsAny<IRequestable>())).Returns(false);
            instance.Process(requestMock);
            requestMocker.VerifySet(x => x.IsSuccess = false);
        }
    }
}