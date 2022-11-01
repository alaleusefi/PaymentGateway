using System;

namespace Core
{
    public interface IReportFactory
    {
        Report Make(Query q, IRequestable request);
    }
    public class ReportFactory : IReportFactory
    {
        private const string OBSCURE_PART = "****-****-****";
        private const string BLANK_CARD_NO = "-";
        public Report Make(Query q, IRequestable request)
        {
            if (request == null)
                return CreateBlank();
            if (q.MerchantId != request.MerchantId)
                return CreateBlank();
            return Create(request);
        }

        private Report Create(IRequestable req)
        {
            var instance = new Report();
            if (req.IsValid)
                instance.Status = req.IsSuccess.Value ? Status.Success : Status.Declined;
            else
                instance.Status = Status.Invalid;
            instance.MerchantId = req.MerchantId;
            instance.Amount = req.Amount;
            instance.Card_Number = OBSCURE_PART + "-" + req.Card.Number.Substring(14);
            instance.Card_Cvv = req.Card.Cvv;
            instance.Card_Currency = req.Card.Currency;
            instance.Card_Expiry_Year = req.Card.Expiry.Year;
            instance.Card_Expiry_Month = req.Card.Expiry.Month;
            return instance;
        }

        private Report CreateBlank()
        {
            var instance = new Report();
            instance.Status = Status.Invalid;
            instance.MerchantId = -1;
            instance.Amount = 0;
            instance.Card_Number = "-";
            instance.Card_Cvv = 0;
            instance.Card_Currency = null;
            instance.Card_Expiry_Year = 0;
            instance.Card_Expiry_Month = 0;
            return instance;
        }
    }
}