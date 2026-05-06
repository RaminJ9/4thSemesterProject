using Core.Repositories;
using Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    public class MachineServiceTests
    {
        public IMachineRepository mrepo;
        public IProductionRepository prepo;
        public MachineService serv;
        [SetUp]
        public void Setup()
        {
            mrepo = new MachineRepository();
            prepo = new ProductionRepository(mrepo);
            serv = new MachineService(mrepo, prepo);

            mrepo.AddMachine(new MockMachine.MockMachine(Guid.NewGuid().ToString(), "Test", "Test"));
            mrepo.AddMachine(new MockMachine.MockMachine(Guid.NewGuid().ToString(), "Test", "Test"));
            mrepo.AddMachine(new MockMachine.MockMachine(Guid.NewGuid().ToString(), "Test", "Test"));
        }
        [Test]
        public void GetMachinesTest()
        {
            Assert.That(serv.GetMachines(), Is.EqualTo(mrepo.Machines));
        }
        [Test]
        public void AddMachineTest()
        {
            var machine = new MockMachine.MockMachine(Guid.NewGuid().ToString(), "New", "New");
            Assert.That(mrepo.Machines.Contains(machine), Is.False);
            serv.AddMachine(machine);
            Assert.That(mrepo.Machines.Contains(machine), Is.True);
        }
        [Test]
        public void RemoveMachineTest()
        {
            var machine = mrepo.Machines[0];
            var machineOther = mrepo.Machines[1];
            var machineOther1 = mrepo.Machines[2];
            serv.RemoveMachine(machine.Guid);
            Assert.Multiple(() =>
            {
                Assert.That(mrepo.Machines.Contains(machine), Is.False);
                Assert.That(mrepo.Machines.Contains(machineOther), Is.True);
                Assert.That(mrepo.Machines.Contains(machineOther1), Is.True);
            });
        }
    }
}
