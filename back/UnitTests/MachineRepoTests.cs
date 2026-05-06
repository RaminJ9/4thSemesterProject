using Common.Contracts;
using Core.Repositories;
using MockMachine;

namespace UnitTests
{
    public class MachineRepoTests
    {
        private IMachineRepository repo;

        [SetUp]
        public void Setup()
        {
            repo = new MachineRepository();
        }

        [Test]
        public void AddMachineTestBasic()
        {
            var machine = new MockMachine.MockMachine(Guid.NewGuid().ToString(), "test", "test");
            var machineNotInRepo = new MockMachine.MockMachine(Guid.NewGuid().ToString(), "test", "test");
            repo.AddMachine(machine);
            Assert.Multiple(() =>
            {
                Assert.That(repo.Machines.Contains(machine), Is.True);
                Assert.That(repo.Machines.Contains(machineNotInRepo), Is.False);
            });
        }
        [Test]
        public void AddMachineDuplicateMachineTest()
        {
            var machine = new MockMachine.MockMachine(Guid.NewGuid().ToString(), "test", "test");
            repo.AddMachine(machine);
            try
            {
                repo.AddMachine(machine);
            } catch
            {
                Assert.Pass();
                return;
            }
            Assert.Fail();
            
        }
        [Test]
        public void AddMachineLocksTest()
        {
            var machine = new MockMachine.MockMachine(Guid.NewGuid().ToString(), "test", "test");

            int threadCount = 10;
            var barrier = new Barrier(threadCount);
            var tasks = new List<Task>();

            for (int i = 0; i < threadCount; i++)
            {
                tasks.Add(Task.Run(() =>
                {
                    barrier.SignalAndWait();
                    try
                    {
                     repo.AddMachine(machine);
                    }
                    catch { }
                }));
            }

            Task.WaitAll(tasks.ToArray());

            Assert.That(repo.Machines.Count, Is.EqualTo(1));

        }
        [Test]
        public void RemoveMachineTest()
        {
            var guid = Guid.NewGuid().ToString();
            var guidNotRemoved = Guid.NewGuid().ToString();
            var machine = new MockMachine.MockMachine(guid, "test", "test");
            var machineNotRemoved = new MockMachine.MockMachine(guidNotRemoved, "test", "test");
            repo.AddMachine(machine);
            repo.AddMachine(machineNotRemoved);
            repo.RemoveMachine(guid);
            Assert.Multiple(() =>
            {
                Assert.That(repo.Machines.Contains(machine), Is.False);
                Assert.That(repo.Machines.Contains(machineNotRemoved), Is.True);
            });
        }
        [Test]
        public void MachineExistsTest()
        {
            var guid = Guid.NewGuid().ToString();
            var guidNotInRepo = Guid.NewGuid().ToString();
            var machine = new MockMachine.MockMachine(guid, "test", "test");
            var machineNotInRepo = new MockMachine.MockMachine(guidNotInRepo, "test", "test");
            repo.AddMachine(machine);
            Assert.Multiple(() =>
            {
                Assert.That(repo.MachineExists(machine), Is.True);
                Assert.That(repo.MachineExists(guid), Is.True);
                Assert.That(repo.MachineExists(machineNotInRepo), Is.False);
                Assert.That(repo.MachineExists(guidNotInRepo), Is.False);
            });
        }
    }
}