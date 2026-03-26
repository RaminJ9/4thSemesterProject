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