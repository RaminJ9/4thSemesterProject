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
            if (ProductionRepository.MachineExistsInProduction(guid))
                throw new UnsafeOperationException($"Machine '{guid}' cannot be removed, since it is used in the production.\nRemove the machine from the production, before removing it entirely.");

            MachineRepository.RemoveMachine(guid); // Not found error thrown in repo
        }
    }
}
