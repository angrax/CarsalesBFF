using CarsalesBFF.HttpClient;
using CarsalesBFF.HttpClient.Character;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarsalesBFF.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CharacterController : ControllerBase
    {
        private readonly RymService _rymService;
        public CharacterController(RymService rymService)
        {
            _rymService = rymService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var character = await _rymService.GetCharacter(id);
            return Ok(character);
        }

        [HttpGet("GetByIds")]
        public async Task<Character[]> GetByIds([FromQuery]string[] ids)
        {
            var characters = await _rymService.GetCharacters(ids);
            return characters;
        }
    }
}
