using Common.Contracts;
using Common.Models;
using Core.Enums;
using Core.Exceptions;
using Core.Repositories;
using System.Runtime.CompilerServices;

namespace Core.Services
{
    // Todo: locks
    public class MachineService
    {
        public List<MachineComponentBase> GetMachines() => MachineRepository.Machines;

        /// <exception cref="DuplicateMachineException">
        /// Thrown when machine already exists.
        /// </exception>
        public void AddMachine(MachineComponentBase machine) => MachineRepository.AddMachine(machine);

        /// <exception cref="MachineNotFoundException">
        /// Thrown when machine to remove wasn't found.
        /// </exception>
        public void RemoveMachine(string guid)
        {
            /// Todo: 
            /// Cant remove machine when production running
            /// Cant remove machine used in production
            MachineRepository.RemoveMachine(guid); // Not found error thrown in repo
        }
    }
}
