using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class AssemblyOperationMessage
    {
        public int ProcessID { get; set; }
    }

    public class AssemblyStatusMessage
    {
        public int LastOperation { get; set; }
        public int CurrentOperation { get; set; }
        public int State { get; set; }
        public string TimeStamp { get; set; } = string.Empty;
    }

    public class AssemblyHealthMessage
    {
        public bool Healthy { get; set; }
        public string? Message { get; set; }
    }
}