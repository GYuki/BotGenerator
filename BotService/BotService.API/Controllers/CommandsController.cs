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
        private readonly ICommandRepository _commandRepository;

        public CommandsController(ICommandRepository commandRepository)
        {
            _commandRepository = commandRepository;
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
            
            var command = await _commandRepository.GetCommandAsync(id);

            if (command != null)
                return Ok(command);
            
            return NotFound();
        }

        [HttpGet]
        [Route("ofbot/{id}")]
        [ProducesResponseType(typeof(List<Bot>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<List<Command>>> GetCommandsByBotIdAsync(int id)
        {
            if (id <= 0)
                return BadRequest();

            var result = await _commandRepository.GetBotCommandsAsync(id);
            return Ok(result);
        }

        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Command), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Command>> UpdateCommandResponseAsync([FromBody] Command _command)
        {
            if (_command is null)
                return BadRequest();
            
            var command = await _commandRepository.UpdateCommandResponseAsync(_command);

            if (command is null)
                return NotFound();

            return Ok(command);
        }

        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> DeleteCommandOfBotAsync(int commandId)
        {
            if (commandId <= 0)
                return BadRequest();
            
            var deleteResult = await _commandRepository.DeleteCommandAsync(commandId);

            if (!deleteResult)
                return NotFound();

            return NoContent();
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult> CreateCommandAsync([FromBody]Command command)
        {
            if (command is null)
                return BadRequest();
            
            await _commandRepository.CreateCommandAsync(command);

            return Ok();
        }
    }
}