using Common.Contracts;
using Core.Enums;
using Core.Exceptions;

namespace Core.Repositories
{
    public interface IProductionRepository
    {
        public List<List<MachineComponentBase>> Production { get; }
        public int Count { get;  }
        public ProductionStates State { get; set; }
        /// <exception cref="MachineNotFoundException">
        /// Thrown when one or more machines in production line wasn't found.
        /// </exception>
        /// /// <exception cref="InvalidProductionException">
        /// Thrown when production line didn't have at least two machines.
        /// </exception>
        public void SetProduction(List<List<MachineComponentBase>> production);
        public bool MachineExistsInProduction(string guid);
    }
}
