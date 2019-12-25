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
        private readonly BotContext _botcontext;

        public BotsController(BotContext context)
        {
            _botcontext = context ?? throw new ArgumentException(nameof(context));
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
            
            var bot = await _botcontext.Bots.SingleOrDefaultAsync(b => b.Id == id);

            if (bot != null)
                return bot;
            
            return NotFound();
        }

        [HttpGet]
        [Route("ofowner/{ownerid:string}")]
        [ProducesResponseType(typeof(List<Bot>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<Bot>>> BotsOfOwnerAsync(string ownerId)
        {
            return await _botcontext.Bots.Where(b => b.Owner.SenderId == ownerId).ToListAsync();
        }

        [HttpDelete]
        [Route("{botname:string}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> DeleteBotAsync(string name)
        {
            var bot = await _botcontext.Bots.Where(b => b.Name == name).FirstOrDefaultAsync();

            if (bot is null)
                return NotFound();
            
            _botcontext.Bots.Remove(bot);

            await _botcontext.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult> CreateBotBySenderIdAsync([FromBody]CreateBotRequest createBotRequest)
        {
            if (string.IsNullOrEmpty(createBotRequest.OwnerId))
                return BadRequest();

            var user = await _botcontext.Users.Where(u => u.SenderId == createBotRequest.OwnerId).FirstOrDefaultAsync();

            if (user is null)
                return StatusCode(500);

            var bot = new Bot
            {
                Owner = user,
                Token = createBotRequest.Token,
                Name = createBotRequest.Name
            };

            _botcontext.Add(bot);

            await _botcontext.SaveChangesAsync();

            return CreatedAtAction(nameof(BotByIdAsync), new { id = bot.Id }, null);
        }
    }
}