using Common.Contracts;
using Common.Models;
using Core.Enums;
using Core.Exceptions;
using Core.Repositories;
using System.Runtime.CompilerServices;

namespace Core.Services
{
    // Todo: locks
    public class ProductionService
    {
        IProductionRepository _productionRepo;
        private Task? productionLoopTask;
        private CancellationTokenSource? cts;

        public ProductionService(IProductionRepository productionRepo)
        {
            _productionRepo = productionRepo;
        }

        /// <exception cref="ImpossibleProductionStateException">
        /// Thrown when start of production was impossible
        /// </exception>
        public void Start()
        {
            if (_productionRepo.State == ProductionStates.Error)
                throw new ImpossibleProductionStateException(ProductionStates.Running, "Cannot start production in state: 'Error'");
            if (_productionRepo.State == ProductionStates.Running)
                throw new ImpossibleProductionStateException(ProductionStates.Running, "Cannot start production in state: 'Running'");

            cts = new();
            productionLoopTask = new Task(RunProductionLoop, cts.Token);
            productionLoopTask.Start();

            _productionRepo.State = ProductionStates.Running;
        }
        public void Stop()
        {
            cts?.Cancel(); // Dont care if cts exists. Production can always reach 'Stopped' state
            _productionRepo.State = ProductionStates.Stopped;
        }

        /// <exception cref="Exception">
        /// Generic exception thrown when machine encounters errors
        /// </exception>
        private async void RunProductionLoop()
        {
            // The actual loop of produciton
            async Task ProductionLoop()
            {
                /// Implicit contract: 
                /// First component in line must find tray based on name
                /// instead of by Id.
                /// (Ask Ramin, Freya or Joachim)
                Tray? tray = new(-1, "Item"); // Always take parts in beginning
                for (int i = 0; i < _productionRepo.Count; i++)
                {
                    MachineComponentBase machine = _productionRepo.Production[i][0]; // Add logic to hanlde multiple machine in each 

                    if (tray == null)
                    {
                        throw new NotImplementedException("No null handling implemented");
                    }

                    // First machine in line shouldnt recieve any prior tray
                    if (i != 0) await machine.Receive(tray);

                    // Last machine shoulndt provide tray further
                    if (i == _productionRepo.Count - 1) continue;
                    tray = await machine.Provide(tray);
                }
            }

            // Run the loop and make sure it can be exited
            while (!cts!.IsCancellationRequested) // cts will never be null. See Start()
            {
                try
                {
                    await ProductionLoop();
                } catch (Exception ex)
                {
                    // Add some sort of error logging
                    _productionRepo.State = ProductionStates.Error;
                    cts.Cancel();
                }
            }
        }

        /// <exception cref="MachineNotFoundException">
        /// Thrown when one or more machines in production line wasn't found.
        /// </exception>
        /// /// <exception cref="InvalidProductionException">
        /// Thrown when production line didn't have at least two machines.
        /// </exception>
        public void SetProduction(List<List<MachineComponentBase>> production)
        {
            if (_productionRepo.State == ProductionStates.Running)
                throw new UnsafeOperationException("Cannot edit production whilst running");
            _productionRepo.SetProduction(production);
        }
        public List<List<MachineComponentBase>> GetProduction() => _productionRepo.Production;
    }
}
