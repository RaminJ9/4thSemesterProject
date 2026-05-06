using Common.Contracts;
using Core.Exceptions;
using System.Reflection.PortableExecutable;

namespace Core.Repositories
{
    public class MachineRepository : IMachineRepository
    {
        public MachineRepository(){}
        public List<MachineComponentBase> Machines { get; private set; } = new();
        private readonly object _writeMachineLock = new object();
        public void AddMachine(MachineComponentBase machine)
        {
            lock (_writeMachineLock)
            {
                if (MachineExists(machine)) throw new DuplicateMachineException(machine);
                Machines.Add(machine);
            }
        }
        public void RemoveMachine(string guid)
        {
            lock (_writeMachineLock)
            {
                int index = Machines.FindIndex(m => m.Guid == guid);
                if (index == -1) throw new MachineNotFoundException(guid); // -1 because FindIndex return -1 when not found
                Machines.RemoveAt(index);
            }
        }
        public void RemoveMachine(MachineComponentBase machine) => RemoveMachine(machine.Guid);
        public bool MachineExists(MachineComponentBase machine) => Machines.Any(m => m.Guid == machine.Guid || m.ConnectionString == machine.ConnectionString);
        public bool MachineExists(string guid) => Machines.Any(m => m.Guid == guid);

    }
}
