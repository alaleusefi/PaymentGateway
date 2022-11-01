using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Core
{
    public class Query
    {
        public int MerchantId { get; set; }
        public int RequestId { get; set; }
    }
}