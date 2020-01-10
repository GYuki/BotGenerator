using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BotService.API.Model;
using BotService.API.Infrastructure;
using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BotService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BotsController : Controller
    {
        private readonly IBotRepository _botRepository;

        public BotsController(IBotRepository botRepository)
        {
            _botRepository = botRepository;
        }

        [HttpGet]
        [Route("{id:int}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Bot), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Bot>> BotByIdAsync(int id)
        {
            if (id <= 0)
                return BadRequest();
            
            var bot = await _botRepository.GetBotAsync(id);

            if (bot != null)
                return Ok(bot);
            
            return NotFound();
        }

        [HttpGet]
        [Route("ofowner/{ownerid}")]
        [ProducesResponseType(typeof(List<Bot>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<Bot>>> BotsOfOwnerAsync(int ownerId)
        {
            var result = await _botRepository.GetBotsOfOwnerAsync(ownerId);
            return Ok(result);
        }

        [HttpDelete]
        [Route("{botId}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> DeleteBotAsync(int botId)
        {
            var deleteResult = await _botRepository.DeleteBotAsync(botId);

            if (!deleteResult)
                return NotFound();

            return NoContent();
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> CreateBotAsync([FromBody]Bot bot)
        {
            if (bot is null)
                return BadRequest();

           await _botRepository.CreateBotAsync(bot);

            return Ok();
        }
    }
}