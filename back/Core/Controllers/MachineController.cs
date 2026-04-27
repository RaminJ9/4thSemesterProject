using Common.Contracts;
using Common.Models;
using Core.Dtos;
using Core.Exceptions;
using Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace Core.Controllers
{
    [ApiController]
    [Route("api/machine")]
    public class MachineController : Controller
    {
        readonly MachineService _machineService;
        readonly ProductionService _productionService;
        private readonly IEnumerable<Type> _components;
        public MachineController(MachineService machineService, ProductionService productionService, IEnumerable<Type> components)
        {
            _machineService = machineService;
            _productionService = productionService;
            _components = components;
        }

        [HttpGet()]
        [ProducesResponseType(typeof(IEnumerable<GetMachineDto>), 200)]
        public async Task<ActionResult<IEnumerable<GetMachineDto>>> GetMachine()
        {
            List<GetMachineDto> machines = GetMachineDto.FromMachine(_machineService.GetMachines());
            return Ok(machines);
        }

        [HttpPost()]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(typeof(string), 200)]
        public async Task<ActionResult<string>> AddMachine([FromBody] PostMachineDto machineDto)
        {
            // Most of this logic should be moved to service, ideally
            Type? type = _components.FirstOrDefault(t => t.ToString() == machineDto.Component);
            if (type == null)
            {
                return NotFound($"Component type wasnt found.\nHas to be one of following:\n{string.Join(", ", _components)}");
            }
            var instance = (MachineComponentBase)Activator.CreateInstance(type, Guid.NewGuid().ToString(), machineDto.Name, machineDto.ConnectionString);
            if (instance is not MachineComponentBase machine)
            {
                return BadRequest("Failed to create component instance");
            }

            try
            {
                _machineService.AddMachine(machine);
            }
            catch (DuplicateMachineException ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(instance.Guid);
        }

        [HttpDelete("{machineGuid}")]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> DeleteMachine(string machineGuid)
        {
            try
            {
                _machineService.RemoveMachine(machineGuid);
            }
            catch (MachineNotFoundException ex)
            {
                return NotFound(ex.Message);
            }

            return Ok();
        }

        [HttpGet("errors")]
        public async Task ErrorSSE(CancellationToken cancellationToken)
        {
            Response.ContentType = "text/event-stream";
            Response.Headers.Append("Cache-Control", "no-cache");
            Response.Headers.Append("X-Accel-Buffering", "no");

            var tcs = new TaskCompletionSource<bool>();

            // Actual method invoked on exception
            async void SendUpdate(Exception e)
            {
                try
                {
                    await Response.WriteAsync($"data: {e.Message}\n\n", cancellationToken);
                    await Response.Body.FlushAsync(cancellationToken);
                }
                catch
                {
                    tcs.TrySetResult(true); // close connection
                }
            }

            _productionService.OnMachineError += SendUpdate;

            // Keep connection alive until cancelled or error occurs
            try
            {
                using var registration = cancellationToken.Register(() => tcs.TrySetResult(true));
                await tcs.Task;
            }
            finally
            {
                _productionService.OnMachineError -= SendUpdate;
            }
        }
    }
}
