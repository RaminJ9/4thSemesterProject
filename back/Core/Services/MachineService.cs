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
        IMachineRepository _machineRepo;
        IProductionRepository _productionRepo;
        public MachineService(IMachineRepository machineRepository, IProductionRepository productionRepository)
        {
            _machineRepo = machineRepository;
            _productionRepo = productionRepository;
        }
        public List<MachineComponentBase> GetMachines() => _machineRepo.Machines;

        /// <exception cref="DuplicateMachineException">
        /// Thrown when machine already exists.
        /// </exception>
        public void AddMachine(MachineComponentBase machine) => _machineRepo.AddMachine(machine);

        /// <exception cref="MachineNotFoundException">
        /// Thrown when machine to remove wasn't found.
        /// </exception>
        public void RemoveMachine(string guid)
        {
            if (_productionRepo.MachineExistsInProduction(guid))
                throw new UnsafeOperationException($"Machine '{guid}' cannot be removed, since it is used in the production.\nRemove the machine from the production, before removing it entirely.");

            _machineRepo.RemoveMachine(guid); // Not found error thrown in repo
        }
    }
}
