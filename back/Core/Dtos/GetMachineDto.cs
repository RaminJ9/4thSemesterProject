using Common.Contracts;

namespace Core.Dtos
{
    public class GetMachineDto
    {
        public string Name { get; set; }
        public string ConnectionString { get; set; }
        public string Guid { get; set; }
        public string Component { get; set; }

        public static GetMachineDto FromMachine(MachineComponentBase machine)
        {
            return new GetMachineDto { Name = machine.Name, ConnectionString = machine.ConnectionString, Guid = machine.Guid, Component = machine.GetType().ToString() };
        }
        public static List<GetMachineDto> FromMachine(List<MachineComponentBase> machines)
        {
            return machines.ConvertAll(x => FromMachine(x));
        }
    }
}
