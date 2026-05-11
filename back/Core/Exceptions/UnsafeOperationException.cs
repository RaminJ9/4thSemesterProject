using Core.Enums;

namespace Core.Exceptions
{
    public class UnsafeOperationException : Exception
    {
        public UnsafeOperationException(string reason) : base(reason){}
    }
}
