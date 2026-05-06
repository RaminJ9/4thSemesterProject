using Common.Contracts;
using Core.Enums;
using Core.Exceptions;
using System.Runtime.CompilerServices;

namespace Core.Repositories
{
    public class ProductionRepository : IProductionRepository
    {
        IMachineRepository _machineRepo;
        public ProductionRepository(IMachineRepository machineRepository)
        {
            _machineRepo = machineRepository;
        }
        public List<List<MachineComponentBase>> Production { get; private set; } = new();
        private HashSet<string> guids = new();
        public int Count => Production.Count;
        private readonly object _writeProductionLock = new object();
        private readonly object _writeStateLock = new object();

        // private set in future?
        private ProductionStates state = ProductionStates.Stopped;
        public ProductionStates State
        {
            get => state;
            set
            {
                lock(_writeStateLock)
                {
                    state = value;
                }
            }
        }
        public void SetProduction(List<List<MachineComponentBase>> production)
        {
            lock(_writeProductionLock)
            {
                HashSet<string> guids = new(); // will overwrite ProductionRepository.guids later

                // Validate that all machines exist
                foreach (List<MachineComponentBase> machinesList in production)
                {
                    foreach (MachineComponentBase machine in machinesList)
                    {
                        guids.Add(machine.Guid);
                        if (!_machineRepo.MachineExists(machine))
                        {
                            throw new MachineNotFoundException(machine.Guid);
                        }
                    }
                }

                // Validate that at least two machines exist
                if (guids.Count < 2)
                {
                    throw new InvalidProductionException("Production must have at least two machines");
                }

                this.guids = guids;
                Production = production;
            }
        }
        public bool MachineExistsInProduction(string guid) => guids.Contains(guid);
    }
}
