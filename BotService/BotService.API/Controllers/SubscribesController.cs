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
        private readonly BotContext _botContext;

        public SubscribesController(BotContext context)
        {
            _botContext = context ?? throw new ArgumentException(nameof(context));
        }

        [HttpGet]
        [Route("{id:int}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Subscribe), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Subscribe>> SubscribeByIdAsync(int id)
        {
            if (id <= 0)
                return NotFound();
            
            var subscribe = await _botContext.Subscribes.SingleOrDefaultAsync(s => s.Id == id);

            if (subscribe != null)
                return subscribe;
            
            return NotFound();
        }

        [HttpGet]
        [Route("subscribers/{bot}")]
        [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<string>>> GetSubscribersAsync(string botName)
        {
            return await _botContext.Subscribes
                        .Where(s => s.Bot.Name == botName)
                        .Select(s => s.ChatId)
                        .ToListAsync();
        }

        [HttpDelete]
        [Route("{botid:int}/{chatid}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> DeleteSubscribeAsync(int botId, string chatId)
        {
            var subscribe = await _botContext.Subscribes.SingleOrDefaultAsync(s => s.BotId == botId && s.ChatId == chatId);

            if (subscribe is null)
                return NotFound();
            
            _botContext.Remove(subscribe);
            await _botContext.SaveChangesAsync();

            return NoContent();
        }

        

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> SubscribeAsync([FromBody]Subscribe subscribe)
        {
            var bot = await _botContext.Bots.SingleOrDefaultAsync(b => b.Name == subscribe.Bot.Name);

            if (bot is null)
                return NotFound();
            
            var _subscribe = new Subscribe
            {
                Bot = bot,
                ChatId = subscribe.ChatId
            };

            _botContext.Subscribes.Add(_subscribe);

            await _botContext.SaveChangesAsync();

            return CreatedAtAction(nameof(SubscribeByIdAsync), new { id = subscribe.Id }, null);
        }
    }

}