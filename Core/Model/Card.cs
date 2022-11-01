using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Core
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Currency { GBP, USD, EUR }

    public interface ICard : IValidatable
    {
        string Number { get; }
        IDate Expiry { get; }
        int Cvv { get; }
        Currency Currency { get; }
    }

    public class Card : ICard
    {
        private const int MIN_CVV = 100;
        private const int MAX_CVV = 999;
        private Card() { }
        public string Number { get; private set; }
        public IDate Expiry { get; private set; }
        public int Cvv { get; private set; }
        public Currency Currency { get; private set; }

        public static Card Create(string number, IDate date, int cvv, Currency currency)
        {
            var instance = new Card
            {
                Number = number,
                Expiry = date,
                Currency = currency,
                Cvv = cvv
            };
            return instance;
        }

        public bool IsValid
        {
            get
            {
                if (Expiry.IsValid == false || Expiry.IsPassed)
                    return false;
                if (Cvv < MIN_CVV || Cvv > MAX_CVV)
                    return false;
                return true;
            }
        }
    }
}