using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Core
{
    public class Report
    {
        public Status Status { get; set; }
        public int Id { get; set; }
        public int MerchantId { get; set; }
        public Decimal Amount { get; set; }
        public string Card_Number { get; set; }
        public int Card_Cvv { get; set; }
        public Currency? Card_Currency { get; set; }
        public int Card_Expiry_Year { get; set; }
        public int Card_Expiry_Month { get; set; }
    }
}