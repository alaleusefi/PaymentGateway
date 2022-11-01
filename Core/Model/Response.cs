using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Core
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Status { Invalid, Success, Declined }
    public class Response
    {
        public Status Status { get; private set; }
        public int Id { get; private set; }

        public Response(int id, Status status)
        {
            Id = id;
            Status = status;
        }
    }
}