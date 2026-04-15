using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Contracts
{
    public abstract class MachineComponentBase
    {
        public readonly string Guid;
        public readonly string Name;
        public readonly string ConnectionString;

        protected MachineComponentBase(string guid, string name, string connectionString)
        {
            Guid = guid;
            Name = name;
            ConnectionString = connectionString;
        }
        public abstract Task<Tray?> Provide(Tray tray);
        public abstract Task<Tray?> Receive(Tray tray);
    }
}
