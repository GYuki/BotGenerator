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
    public class CommandsController : Controller
    {
        private readonly BotContext _botContext;

        public CommandsController(BotContext context)
        {
            _botContext = context ?? throw new ArgumentException(nameof(context));
        }

        [HttpGet]
        [Route("{id:int}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Bot), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Command>> CommandByIdAsync(int id)
        {
            if (id <= 0)
                return BadRequest();
            
            var command = await _botContext.Commands.SingleOrDefaultAsync(c => c.Id == id);

            if (command != null)
                return command;
            
            return NotFound();
        }

        [HttpGet]
        [Route("byname/{name:string}")]
        [ProducesResponseType(typeof(List<Bot>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<Command>>> GetCommandsByBotNameAsync(string name)
        {
            return await _botContext.Commands.Where(c => c.Bot.Name == name).ToListAsync();
        }

        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Command), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Command>> UpdateCommandResponseAsync([FromBody] UpdateCommandRequest request)
        {
            if (request.HasNullField)
                return BadRequest();
            
            var command = await _botContext
                               .Commands
                               .SingleOrDefaultAsync(b => b.Request == request.Request
                               && b.Bot.Name == request.BotName);
            if (command is null)
                return NotFound();
            
            command.Response = request.Response;

            await _botContext.SaveChangesAsync();

            return CreatedAtAction(nameof(CommandByIdAsync), new { id = command.Id }, null);
        }

        [HttpDelete]
        [Route("{commandname:string}/bot/{botname:string}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> DeleteCommandOfBotAsync(string commandName, string botName)
        {
            if (string.IsNullOrEmpty(commandName) || string.IsNullOrEmpty(botName))
                return BadRequest();
            
            var commandToDelete = await _botContext
                                            .Commands
                                            .SingleOrDefaultAsync(c => c.Bot.Name == botName 
                                            && c.Request == commandName);
            if (commandToDelete is null)
                return NotFound();
            
            _botContext.Remove(commandToDelete);
            await _botContext.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult> CreateCommandAsync([FromBody]CreateCommandRequest request)
        {
            if (string.IsNullOrEmpty(request.BotName))
                return BadRequest();
            
            var bot = await _botContext.Bots.Where(b => b.Name == request.BotName).FirstOrDefaultAsync();

            if (bot is null)
                return StatusCode(500);
            
            var command = new Command
            {
                Bot = bot,
                Request = request.Request,
                Response = request.Response
            };

            _botContext.Add(command);

            await _botContext.SaveChangesAsync();

            return CreatedAtAction(nameof(CommandByIdAsync), new { id = command.Id}, null);
        }
    }
}