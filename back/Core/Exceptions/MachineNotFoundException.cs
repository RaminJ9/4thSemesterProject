using Common.Contracts;

namespace Core.Exceptions
{
    public class MachineNotFoundException : Exception
    {
        public MachineNotFoundException(string guid) 
            : base($"Machine with Guid: '{guid}', was not found") {}
    }
}
