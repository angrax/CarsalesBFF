using CarsalesBFF.HttpClient;
using CarsalesBFF.HttpClient.Episode;
using Microsoft.AspNetCore.Mvc;

namespace CarsalesBFF.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EpisodeController : ControllerBase
    {
        private readonly RymService _rymService;
        public EpisodeController(RymService rymService)
        {
            _rymService = rymService;
        }

        [HttpGet("GetPaged")]
        public async Task<Episode> GetPaged(int page = 1)
        {
            return await _rymService.GetEpisode(page);
        }

        [HttpGet("GetAll")]
        public async Task<IEnumerable<EpisodeResponse>> GetAll()
        {
            return await _rymService.GetAllEpisodes();
        }
    }
}
