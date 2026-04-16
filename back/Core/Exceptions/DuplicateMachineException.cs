using Common.Contracts;

namespace Core.Exceptions
{
    public class DuplicateMachineException : Exception
    {
        public DuplicateMachineException(MachineComponentBase machine) 
            : base($"Machine; '{machine.Name}' with Guid: '{machine.Guid}', already exists") {}
    }
}
