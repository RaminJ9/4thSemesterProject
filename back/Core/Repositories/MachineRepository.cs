using Common.Contracts;
using Core.Exceptions;
using System.Reflection.PortableExecutable;

namespace Core.Repositories
{
    public static class MachineRepository
    {
        public static List<MachineComponentBase> Machines { get; private set; } = new();

        /// <exception cref="DuplicateMachineException">
        /// Thrown when machine already exists.
        /// </exception>
        public static void AddMachine(MachineComponentBase machine)
        {
            if (MachineExists(machine)) throw new DuplicateMachineException(machine);
            Machines.Add(machine);
        }

        /// <exception cref="MachineNotFoundException">
        /// Thrown when machine to remove wasn't found.
        /// </exception>
        public static void RemoveMachine(string guid)
        {
            int index = Machines.FindIndex(m => m.Guid == guid);
            if (index == -1) throw new MachineNotFoundException(guid); // -1 because FindIndex return -1 when not found
            Machines.RemoveAt(index);
        }
        /// <exception cref="MachineNotFoundException">
        /// Thrown when machine to remove wasn't found.
        /// </exception>
        public static void RemoveMachine(MachineComponentBase machine) => RemoveMachine(machine.Guid);
        public static bool MachineExists(MachineComponentBase machine) => Machines.Any(m => m.Guid == machine.Guid || m.ConnectionString == machine.ConnectionString);
        public static bool MachineExists(string guid) => Machines.Any(m => m.Guid == guid);

    }
}
