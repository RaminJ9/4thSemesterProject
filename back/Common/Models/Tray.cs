using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class Tray(int id, string name)
    {
        public int Id = id;
        public string Name = name;
    }
}
