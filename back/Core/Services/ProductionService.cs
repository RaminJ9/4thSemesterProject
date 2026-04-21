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
        private Task? productionLoopTask;
        private CancellationTokenSource? cts;

        public void Start()
        {
            if (ProductionRepository.State == ProductionStates.Error)
                throw new ImpossibleProductionStateException(ProductionStates.Running, "Cannot start production in state: 'Error'");
            if (ProductionRepository.State == ProductionStates.Running)
                throw new ImpossibleProductionStateException(ProductionStates.Running, "Cannot start production in state: 'Running'");

            cts = new();
            productionLoopTask = new Task(RunProductionLoop, cts.Token);
            productionLoopTask.Start();

            ProductionRepository.State = ProductionStates.Running;
        }
        public void Stop() => cts?.Cancel(); // Dont care if cts exists. Production can always reach 'Stopped' state

        private async void RunProductionLoop()
        {
            while (!cts!.IsCancellationRequested) // cts will never be null. See Start()
            {

                try
                {
                    Tray? tray = new(0, "Temp");
                    for (int i = 0; i < ProductionRepository.Count; i++)
                    {
                        MachineComponentBase machine = ProductionRepository.Production[i][0]; // Add logic to hanlde multiple machine in each 
                        
                        if (tray == null)
                        {
                            throw new NotImplementedException("No null handling implemented");
                        }


                        // First machine in line shouldnt recieve any prior tray
                        if (i != 0) await machine.Receive(tray);

                        // Last machine shoulndt provide tray further
                        if (i == ProductionRepository.Count - 1) continue;
                        tray = await machine.Provide(tray);
                    }
                } catch (Exception ex)
                {
                    ProductionRepository.State = ProductionStates.Error;
                }
            }
        }

        public void SetProduction(List<List<MachineComponentBase>> production)
        {
            // Todo: cant edit production while running
            ProductionRepository.SetProduction(production);
        }
        public List<List<MachineComponentBase>> GetProduction() => ProductionRepository.Production;
        public List<MachineComponentBase> GetMachines() => MachineRepository.Machines;
        public void AddMachine(MachineComponentBase machine) => MachineRepository.AddMachine(machine); // Duplicate error thrown in repo
        public void RemoveMachine(string guid)
        {
            // Todo: Cant remove machine when production running
            MachineRepository.RemoveMachine(guid); // Not found error thrown in repo
        }
    }
}
