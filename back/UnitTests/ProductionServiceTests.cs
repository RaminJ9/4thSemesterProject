using Common.Contracts;
using Core.Repositories;
using Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    public class ProductionServiceTests
    {
        IProductionRepository prepo;
        IMachineRepository mrepo;
        ProductionService serv;
        [SetUp]
        public void Setup()
        {
            mrepo = new MachineRepository();
            prepo = new ProductionRepository(mrepo);
            serv = new ProductionService(prepo);

            mrepo.AddMachine(new MockMachine.MockMachine(Guid.NewGuid().ToString(), "Test", "Test"));
            mrepo.AddMachine(new MockMachine.MockMachine(Guid.NewGuid().ToString(), "Test", "Test"));
            mrepo.AddMachine(new MockMachine.MockMachine(Guid.NewGuid().ToString(), "Test", "Test"));
        }

        [Test]
        public void StartImpossibleInRunningStateTest()
        {
            prepo.State = Core.Enums.ProductionStates.Running;
            try
            {
                serv.Start();
            } catch
            {
                Assert.Pass();
                return;
            }
            Assert.Fail();
        }
        [Test]
        public void StartImpossibleInErrorStateTest()
        {
            prepo.State = Core.Enums.ProductionStates.Error;
            try
            {
                serv.Start();
            }
            catch
            {
                Assert.Pass();
                return;
            }
            Assert.Fail();
        }
        [Test]
        public void StartChangesStateTest()
        {
            prepo.State = Core.Enums.ProductionStates.Stopped;
            serv.Start();
            Assert.That(prepo.State, Is.EqualTo(Core.Enums.ProductionStates.Running));
        }
        [Test]
        public void SetProductionTest()
        {
            List<List<MachineComponentBase>> prod = [[mrepo.Machines[0]], [mrepo.Machines[1]], [mrepo.Machines[2]], [mrepo.Machines[1]], [mrepo.Machines[0]]];
            serv.SetProduction(prod);
            Assert.That(prepo.Production, Is.EqualTo(prod));
        }
        [Test]
        public void GetProductionTest()
        {
            List<List<MachineComponentBase>> prod = [[mrepo.Machines[0]], [mrepo.Machines[1]], [mrepo.Machines[2]], [mrepo.Machines[1]], [mrepo.Machines[0]]];
            prepo.SetProduction(prod);
            Assert.That(serv.GetProduction(), Is.EqualTo(prod));
        }
        [Test]
        public async Task ProductionLoopStatesTest()
        {
            List<List<MachineComponentBase>> prod = [[mrepo.Machines[0]], [mrepo.Machines[1]], [mrepo.Machines[2]], [mrepo.Machines[1]], [mrepo.Machines[0]]];
            prepo.SetProduction(prod);
            serv.Start();
            Assert.That(prepo.State, Is.EqualTo(Core.Enums.ProductionStates.Running));
            serv.Stop();
            //Assert.That(prepo.State, Is.EqualTo(Core.Enums.ProductionStates.Stopped));

            var machine = new MockMachine.MockMachine(Guid.NewGuid().ToString(), "Error Machine", "0;1;-1");
            mrepo.AddMachine(machine);

            prod = [[machine], [mrepo.Machines[1]], [mrepo.Machines[2]], [mrepo.Machines[1]], [mrepo.Machines[0]]];
            prepo.SetProduction(prod);
            serv.Start();
            await Task.Delay(1);
            Assert.That(prepo.State, Is.EqualTo(Core.Enums.ProductionStates.Error));
            serv.Stop();
            Assert.That(prepo.State, Is.EqualTo(Core.Enums.ProductionStates.Stopped));
        }
        [Test]
        public async Task ProductionLoopExecutionConsistencyTest()
        {
            var machineToCauseError = new MockMachine.MockMachine(Guid.NewGuid().ToString(), "Error Machine", "0;10;-1"); // Will provide 10 times before hitting error, but cycle will say 11
            var machine = new MockMachine.MockMachine(Guid.NewGuid().ToString(), "Test", "0;-1;-1"); // no errors
            var machine1 = new MockMachine.MockMachine(Guid.NewGuid().ToString(), "Test1", "0;-1;-1"); // no errors
            mrepo.AddMachine(machineToCauseError);
            mrepo.AddMachine(machine);
            mrepo.AddMachine(machine1);

            List<List<MachineComponentBase>>  prod = [[machineToCauseError], [machine], [machine1]];
            prepo.SetProduction(prod);
            serv.Start();
            var timeout = Task.Delay(10 * 1000);
            while (prepo.State != Core.Enums.ProductionStates.Error && !timeout.IsCompleted)
            {
                await Task.Delay(5);
            }
            if (timeout.IsCompleted)
            {
                throw new Exception("Timeout reached");
            }
            Assert.That(machineToCauseError.provideCycle, Is.EqualTo(10)); // Set to cause error at 11            
            Assert.That(machineToCauseError.receiveCycle, Is.EqualTo(1)); // First machine should never receive (when only placed at start)
            Assert.That(machine.provideCycle, Is.EqualTo(10));  
            Assert.That(machine.receiveCycle, Is.EqualTo(10));  
            Assert.That(machine1.provideCycle, Is.EqualTo(1)); // Last machine should never provide (when only placed at end)
            Assert.That(machine1.receiveCycle, Is.EqualTo(10));  
        }
    }
}
