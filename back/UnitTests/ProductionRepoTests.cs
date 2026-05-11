using Common.Contracts;
using Core.Repositories;
using NuGet.Frameworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    public class ProductionRepoTests
    {
        private IProductionRepository repo;
        private IMachineRepository mrepo;

        [SetUp]
        public void Setup()
        {
            mrepo = new MachineRepository();
            repo = new ProductionRepository(mrepo);
            var machine = new MockMachine.MockMachine(Guid.NewGuid().ToString(), "test", "test");
            var machine1 = new MockMachine.MockMachine(Guid.NewGuid().ToString(), "test1", "test1");
            var machine2 = new MockMachine.MockMachine(Guid.NewGuid().ToString(), "test2", "test2");
            var machine3 = new MockMachine.MockMachine(Guid.NewGuid().ToString(), "test3", "test3");
            mrepo.AddMachine(machine);
            mrepo.AddMachine(machine1);
            mrepo.AddMachine(machine2);
            mrepo.AddMachine(machine3);
        }
        [Test]
        public void SetProductionCantAddNonExistentMachinesTest()
        {
            List<List<MachineComponentBase>> prod = [[mrepo.Machines[0]], [mrepo.Machines[1]], [mrepo.Machines[2]], [new MockMachine.MockMachine(Guid.NewGuid().ToString(), "New", "New")]];
            try
            {
                repo.SetProduction(prod);
            } catch
            {
                Assert.Pass();
                return;
            }
            Assert.Fail();
        }
        [Test]
        public void SetProductionLessThanTwoMachinesTest()
        {
            List<List<MachineComponentBase>> prod = [[mrepo.Machines[0]]];
            try
            {
                repo.SetProduction(prod);
            } catch
            {
                Assert.Pass();
                return;
            }
            Assert.Fail();
        }
        [Test]
        public void SetProductionGuidsNotLostOrCreatedTest()
        {
            List<List<MachineComponentBase>> prod = [[mrepo.Machines[0]], [mrepo.Machines[1]], [mrepo.Machines[2]]];
            repo.SetProduction(prod);
            Assert.That(repo.Production.Count, Is.EqualTo(3));
        }

        [Test]
        public void MachineExistsInProductionTest()
        {
            List<List<MachineComponentBase>> prod = [[mrepo.Machines[0]], [mrepo.Machines[1]], [mrepo.Machines[2]]];
            repo.SetProduction(prod);
            Assert.Multiple(() =>
            {
                Assert.That(repo.MachineExistsInProduction(mrepo.Machines[0].Guid), Is.True);
                Assert.That(repo.MachineExistsInProduction(mrepo.Machines[1].Guid), Is.True);
                Assert.That(repo.MachineExistsInProduction(mrepo.Machines[2].Guid), Is.True);
                Assert.That(repo.MachineExistsInProduction(mrepo.Machines[3].Guid), Is.False);
            });
        }
    }
}
