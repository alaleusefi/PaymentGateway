using System;

namespace Core
{
    public interface IResponseFactory
    {
        Response Process(IRequestable request);
    }

    public class ResponseFactory : IResponseFactory
    {
        private readonly IBank bank;
        public ResponseFactory(IBank b)
        {
            bank = b;
        }

        public Response Process(IRequestable request)
        {
            if (request.IsSuccess.HasValue)
                throw new ArgumentException("Can process fresh requests only!");

            bool? success = null;
            if (request.IsValid) success = bank.Pay(request);
            request.IsSuccess = success;

            Status responseStatus;
            if (request.IsValid == false)
                responseStatus = Status.Invalid;
            else
                responseStatus = success.Value ? Status.Success : Status.Declined;

            return new Response(request.Id, responseStatus);
        }
    }
}