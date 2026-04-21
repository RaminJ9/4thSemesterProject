using Common.Contracts;

namespace Core.Exceptions
{
    public class InvalidProductionException : Exception
    {
        public InvalidProductionException(string message) 
            : base(message) {}
    }
}
