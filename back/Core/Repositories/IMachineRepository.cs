using Common.Contracts;
using Core.Exceptions;

namespace Core.Repositories
{
    public interface IMachineRepository
    {
        public List<MachineComponentBase> Machines { get; }
        /// <exception cref="DuplicateMachineException">
        /// Thrown when machine already exists.
        /// </exception>
        public void AddMachine(MachineComponentBase machine);

        /// <exception cref="MachineNotFoundException">
        /// Thrown when machine to remove wasn't found.
        /// </exception>
        public void RemoveMachine(string guid);
        /// <exception cref="MachineNotFoundException">
        /// Thrown when machine to remove wasn't found.
        /// </exception>
        public void RemoveMachine(MachineComponentBase machine);
        public bool MachineExists(MachineComponentBase machine);
        public bool MachineExists(string guid);
    }
}
