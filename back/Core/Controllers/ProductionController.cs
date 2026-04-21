using Common.Contracts;
using Common.Models;
using Core.Dtos;
using Core.Exceptions;
using Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace Core.Controllers
{
    [ApiController]
    [Route("api/")]
    public class ProductionController : Controller
    {
        readonly ProductionService _productionService;
        private readonly IEnumerable<Type> _components;
        public ProductionController(ProductionService productionService, IEnumerable<Type> components)
        {
            _productionService = productionService;
            _components = components;
        }

        [HttpGet("machine")]
        [ProducesResponseType(typeof(IEnumerable<GetMachineDto>), 200)]
        public async Task<ActionResult<IEnumerable<GetMachineDto>>> GetMachine()
        {
            List<GetMachineDto> machines = GetMachineDto.FromMachine(_productionService.GetMachines());
            return Ok(machines);
        }

        [HttpPost("machine")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> AddMachine([FromBody] PostMachineDto machineDto)
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
                _productionService.AddMachine(machine);
            } catch (DuplicateMachineException ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        [HttpDelete("machine/{machineGuid}")]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> DeleteMachine(string machineGuid)
        {
            try
            {
                _productionService.RemoveMachine(machineGuid);
            } catch(Exception ex)
            {
                return NotFound(ex.Message);
            }

            return Ok();
        }

        [HttpGet("production")]
        [ProducesResponseType(typeof(IEnumerable<IEnumerable<GetMachineDto>>), 200)]
        public async Task<ActionResult<IEnumerable<IEnumerable<GetMachineDto>>>> GetProduction()
        {
            List<List<GetMachineDto>> production = _productionService.GetProduction().ConvertAll(l => GetMachineDto.FromMachine(l));

            return Ok(production);
        }

        [HttpPost("production")]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> SetProduction([FromBody] IEnumerable<IEnumerable<string>> production)
        {
            List<MachineComponentBase> machines = _productionService.GetMachines();

            // Translate strings to MachineComponentBase
            List<List<MachineComponentBase>> newProduction = production.Aggregate(new List<List<MachineComponentBase>>(), (acc, l) =>
            {
                acc.Add(l.Aggregate(new List<MachineComponentBase>(), (ac, guid) =>
                {
                    ac.Add(machines.First(m => m.Guid == guid));
                    return ac;
                }));
                return acc;
            });
            try{ _productionService.SetProduction(newProduction); } 
            catch (MachineNotFoundException ex) { return NotFound(ex.Message); }
            catch (InvalidProductionException ex) { return  BadRequest(ex.Message); }

            return Ok();
        }
        [HttpPost("production/state/{state}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> StartStopProduction(bool state)
        {
            try
            {
                if (state)
                    _productionService.Start();
                else
                    _productionService.Stop();
            } catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        [HttpGet("components")]
        [ProducesResponseType(typeof(string), 200)]
        public async Task<ActionResult<string>> GetComponents()
        {
            return Ok(string.Join(", ", _components));
        }
    }
}
