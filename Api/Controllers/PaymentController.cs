using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Core;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IRequestFactory requestFactory;
        private readonly IResponseFactory responseFactory;
        private readonly IReportFactory reportFactory;
        private readonly IRepository<Request> requestRepo;

        public PaymentController(
            IRequestFactory requester,
            IResponseFactory responser,
            IReportFactory reporter,
            IRepository<Request> repo)
        {
            requestFactory = requester;
            responseFactory = responser;
            reportFactory = reporter;
            requestRepo = repo;
        }

        [HttpPost]
        public Response Post([FromBody] Dto dto)
        {
            var request = requestFactory.Make(dto);
            var response = responseFactory.Process(request);
            return response;
        }

        [HttpGet]
        public Report Get([FromBody] Query query)
        {
            var request = requestRepo.Get(query.RequestId);
            return reportFactory.Make(query, request);
        }
    }
}
