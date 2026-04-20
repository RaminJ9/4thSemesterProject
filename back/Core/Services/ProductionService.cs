using Common.Contracts;
using Common.Models;
using Core.Enums;
using Core.Exceptions;
using Core.Utils;
using System.Runtime.CompilerServices;

namespace Core.Services
{
    public class ProductionService
    {
        private Task? productionLoopTask;
        private CancellationTokenSource cts;
        
        // Below could be moved to repo, but probably not needed
        private List<List<MachineComponentBase>> production = new();
        private List<MachineComponentBase> machines = new();
        private ProductionStates productionState = ProductionStates.Stopped;


        public bool Start()
        {
            if (productionState == ProductionStates.Error)
            {
                throw new ImpossibleProductionStateException(ProductionStates.Running, "Cannot start production in state: 'Error'");
            }
            if (productionState == ProductionStates.Running)
            {
                throw new ImpossibleProductionStateException(ProductionStates.Running, "Cannot start production in state: 'Running'");
            }

            cts = new();
            productionLoopTask = new Task(RunProductionLoop, cts.Token);
            productionLoopTask.Start();

            productionState = ProductionStates.Running;
            return true; // for now
        }
        public bool Stop()
        {
            cts.Cancel();
            return true; // for now
        }

        private async void RunProductionLoop()
        {
            while (!cts.IsCancellationRequested)
            {

                try
                {
                    Tray? tray = new(0, "Temp");
                    for (int i = 0; i < production.Count; i++)
                    {
                        MachineComponentBase machine = production[i][0] // Add logic to hanlde multiple machine in each step
                        ;
                        
                        if (tray == null)
                        {
                            throw new NotImplementedException("No null handling implemented");
                        }


                        // First machine in line shouldnt recieve any prior tray
                        if (i != 0) await machine.Receive(tray);

                        // Last machine shoulndt provide tray further
                        if (i != machines.Count - 1) continue;
                        tray = await machine.Provide(tray);
                    }
                } catch (Exception ex)
                {
                    productionState = ProductionStates.Error;
                }
            }
        }

        public void SetProduction(List<List<MachineComponentBase>> production)
        {
            // Validate that all machines exist
            foreach(List<MachineComponentBase> machinesList in production)
            {
                foreach(MachineComponentBase machine in machinesList)
                {

                    if (!machines.Contains(machine))
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

            this.production = production;
        }
        public List<List<MachineComponentBase>> GetProduction()
        {
            return production;
        }
        public void AddMachine(MachineComponentBase machine)
        {
            // Check for duplicates
            if (machines.Any(m => m.Guid == machine.Guid))
            {
                throw new DuplicateMachineException(machine);
            }

            machines.Add(machine);
        }

        public void RemoveMachine(string guid)
        {
            // Check if machine exists
            MachineComponentBase? machine = machines.Find(macine => macine.Guid == guid);
            if (machine == null)
            {
                throw new MachineNotFoundException(guid);
            }

            // Make sure remove action executes properly
            if (!machines.Remove(machine))
            {
                throw new Exception("Sonething went wrong whilst removing the machine");
            }

        }
    }
}
