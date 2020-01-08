using BotService.API.Model;
using BotService.API.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BotService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscribesController : Controller
    {
        private readonly ISubscribeRepository _subscribeRepository;

        public SubscribesController(ISubscribeRepository subscribeRepository)
        {
            _subscribeRepository = subscribeRepository;
        }

        [HttpGet]
        [Route("subscribers/{bot}")]
        [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<string>>> GetSubscribersAsync(int botId)
        {
            return await _subscribeRepository.GetSubscribersAsync(botId);
        }

        [HttpDelete]
        [Route("{botname:int}/{chatid}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> DeleteSubscribeAsync(string botName, string chatId)
        {
            if (string.IsNullOrEmpty(botName) || string.IsNullOrEmpty(chatId))
                return BadRequest();
            
            var deleteResult = await _subscribeRepository.DeleteSubscriptionAsync(botName, chatId);

            if (!deleteResult)
                return NotFound();

            return NoContent();
        }
        
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> SubscribeAsync([FromBody]Subscribe subscribe)
        {
            if (subscribe is null)
                return BadRequest();
            
            await _subscribeRepository.SubscribeAsync(subscribe);

            return Ok();
        }
    }

}