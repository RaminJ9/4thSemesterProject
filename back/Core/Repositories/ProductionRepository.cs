using Common.Contracts;
using Core.Enums;
using Core.Exceptions;

namespace Core.Repositories
{
    public static class ProductionRepository
    {
        public static List<List<MachineComponentBase>> Production { get; private set; } = new();
        public static int Count => Production.Count;
        public static ProductionStates State { get; set; } = ProductionStates.Stopped; // private set in future?

        /// <exception cref="MachineNotFoundException">
        /// Thrown when one or more machines in production line wasn't found.
        /// </exception>
        /// /// <exception cref="InvalidProductionException">
        /// Thrown when production line didn't have at least two machines.
        /// </exception>
        public static void SetProduction(List<List<MachineComponentBase>> production)
        {
            // Validate that all machines exist
            foreach (List<MachineComponentBase> machinesList in production)
            {
                foreach (MachineComponentBase machine in machinesList)
                {

                    if (!MachineRepository.MachineExists(machine))
                    {
                        throw new MachineNotFoundException(machine.Guid);
                    }
                }
            }

            // Validate that at least two machines exist
            if (production.Count < 2)
            {
                throw new InvalidProductionException("Production must have at least two machines");
            }

            Production = production;
        }

    }
}
