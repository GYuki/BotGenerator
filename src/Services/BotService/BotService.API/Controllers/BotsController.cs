using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BotService.API.Model;
using BotService.API.Infrastructure;
using BotService.API.IntegrationEvents.Events;
using BotService.API.IntegrationEvents;
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
        private readonly IBotIntegrationEventService _botIntegrationEventService;

        public BotsController(IBotRepository botRepository, IBotIntegrationEventService botIntegrationEventService)
        {
            _botRepository = botRepository;
            _botIntegrationEventService = botIntegrationEventService;
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

        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult> UpdateBotTokenAsync([FromBody]Bot botToUpdate)
        {
            var currentBot = await _botRepository.GetBotAsync(botToUpdate.Id);

            if (currentBot == null)
                return NotFound();
            
            var oldToken = currentBot.Token;
            var raiseTokenChangedEvent = oldToken != botToUpdate.Token;

            currentBot.Token = botToUpdate.Token;

            if (raiseTokenChangedEvent)
            {
                var tokenChangedEvent = new TokenChangedIntegrationEvent(currentBot.Token, oldToken);

                await _botRepository.UpdateBotTokenAsync(currentBot);

                await _botIntegrationEventService.SaveEventAndBotContextChangesAsync(tokenChangedEvent, Guid.NewGuid());

                await _botIntegrationEventService.PublishThroughEventBusAsync(tokenChangedEvent);
            }
            else
                await _botRepository.UpdateBotTokenAsync(currentBot);

            return CreatedAtAction(nameof(BotByIdAsync), new { id = currentBot.Id }, null);
        }

        [HttpDelete]
        [Route("{botId}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> DeleteBotAsync(int botId)
        {
            if (botId <= 0)
                return BadRequest();

            var deleteResult = await _botRepository.DeleteBotAsync(botId);

            if (!deleteResult)
                return NotFound();

            return NoContent();
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        public async Task<ActionResult> CreateBotAsync([FromBody]Bot bot)
        {
            if (bot is null)
                return BadRequest();
            
            var checkBot = await _botRepository.GetBotByTokenAsync(bot.Token);

            if (checkBot != null)
                return Conflict();

           await _botRepository.CreateBotAsync(bot);

            return Ok();
        }
    }
}