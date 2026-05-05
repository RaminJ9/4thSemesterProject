using Common.Contracts;
using Common.Models;
using Core.Dtos;
using Core.Exceptions;
using Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace Core.Controllers
{
    [ApiController]
    [Route("api/component")]
    public class ComponentController : Controller
    {
        private readonly IEnumerable<Type> _components;
        public ComponentController(IEnumerable<Type> components)
        {
            _components = components;
        }

        [HttpGet()]
        [ProducesResponseType(typeof(string), 200)]
        public async Task<ActionResult<string>> GetComponents()
        {
            return Ok(string.Join(", ", _components));
        }
    }
}
