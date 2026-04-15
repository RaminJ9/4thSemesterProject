using Common.Contracts;
using Common.Models;
using System.Composition;

[Export(typeof(IMachineComponent))]
public class Example : IMachineComponent
{
    public Task<Tray?> Provide(Tray tray)
    {
        throw new NotImplementedException();
    }

    public Task<Tray?> Receive(Tray tray)
    {
        throw new NotImplementedException();
    }
}
[Export(typeof(MachineComponentBase))]
public class ExampleTwo : MachineComponentBase
{
    public ExampleTwo(string guid, string name, string connectionString) : base(guid, name, connectionString){}
    public override Task<Tray?> Provide(Tray tray)
    {
        throw new NotImplementedException();
    }

    public override Task<Tray?> Receive(Tray tray)
    {
        throw new NotImplementedException();
    }
}