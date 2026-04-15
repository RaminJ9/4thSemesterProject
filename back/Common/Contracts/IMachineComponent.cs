using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Contracts
{
    /// <summary>
    /// Deprecated already?! probably??
    /// </summary>
    public interface IMachineComponent
    {
        public Task<Tray?> Provide(Tray tray);
        public Task<Tray?> Receive(Tray tray);
    }
}
