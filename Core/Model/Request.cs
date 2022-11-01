using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Core
{
    public interface IValidatable { bool IsValid { get; } }
    public interface ISuccessible { bool? IsSuccess { get; set; } }

    public interface IRequestable : IValidatable, ISuccessible
    {
        int Id { get; }
        int MerchantId { get; }
        ICard Card { get; }
        Decimal Amount { get; }
    }

    public class Request : IRequestable
    {
        private const decimal MIN_AMOUNT = 0.5m;
        private const int MAX_AMOUNT = 500;
        private Request() { }
        public int Id { get; private set; }
        public int MerchantId { get; private set; }
        public ICard Card { get; private set; }
        public decimal Amount { get; private set; }
        public bool? IsSuccess { get; set; }

        public static Request Create(int merchantId, ICard card, decimal amount, IRepository<Request> repo)
        {
            var instance = new Request();
            instance.MerchantId = merchantId;
            instance.Card = card;
            instance.Amount = amount;
            instance.Id = repo.Save(instance);
            return instance;
        }

        public bool IsValid
        {
            get
            {
                if (MerchantId < 0)
                    return false;
                if (Card.IsValid == false)
                    return false;
                if (Amount < MIN_AMOUNT || Amount > MAX_AMOUNT)
                    return false;
                return true;
            }
        }
    }
}