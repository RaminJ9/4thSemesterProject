using Common.Contracts;
using Common.Models;
using Core.Dtos;
using Core.Exceptions;
using Core.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Core.Controllers
{
    [ApiController]
    [Route("api/production")]
    public class ProductionController : Controller
    {
        readonly ProductionService _productionService;
        readonly MachineService _machineService;
        public ProductionController(ProductionService productionService, MachineService machineService)
        {
            _productionService = productionService;
            _machineService = machineService;
        }

        [HttpGet()]
        [ProducesResponseType(typeof(IEnumerable<IEnumerable<GetMachineDto>>), 200)]
        public async Task<ActionResult<Dictionary<int, IEnumerable<GetMachineDto>>>> GetProduction()
        {
            List<List<GetMachineDto>> production = _productionService.GetProduction().ConvertAll(l => GetMachineDto.FromMachine(l));
            var formattedProduction = production.Aggregate(new Dictionary<int, IEnumerable<GetMachineDto>>(), (acc, l) =>
            {
                acc.Add(acc.Count, l);
                return acc;
            });

            return Ok(production);
        }

        [HttpPost()]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> SetProduction([FromBody] Dictionary<int, IEnumerable<string>> production)
        {
            var formattedProduction = production.Values.ToList();
            List<MachineComponentBase> machines = _machineService.GetMachines();

            // Translate strings to MachineComponentBase
            List<List<MachineComponentBase>> newProduction = formattedProduction.Aggregate(new List<List<MachineComponentBase>>(), (acc, l) =>
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
        [HttpPost("state/{state}")]
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
            } catch(ImpossibleProductionStateException ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

    }
}
