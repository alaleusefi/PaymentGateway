namespace Core
{
    public interface IBank
    {
        bool Pay(IRequestable request);
    }
    public class FakeBank : IBank
    {
        private const int IMAGINARY_ACCOUNT_BALANCE = 200;
        public bool Pay(IRequestable request)
        {
            return request.Amount <= 200;
        }
    }
}