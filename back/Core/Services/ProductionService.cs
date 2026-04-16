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

            RunProductionLoop();

            return true; // for now
        }
        public bool Stop()
        {
            throw new NotImplementedException();
        }

        private void RunProductionLoop()
        {
            /// This method maybe shouldnt exist
            /// and should instead just be moved to driver
            cts = new();
            productionLoopTask = new Task(RunProductionLoop, cts.Token);
            productionLoopTask.Start();

            productionState = ProductionStates.Running;
            
            while (!cts.IsCancellationRequested)
            {

                try
                {
                    // The work :)
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
