using Microsoft.AspNetCore.Mvc;
using TelegramReceiver.API.Infrastructure.Repositories;
using TelegramReceiver.API.Models;
using System.Threading.Tasks;
using System.Net;


namespace TelegramReceiver.API.Controllers
{
    [Route("receiver/[controller]")]
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
        [ProducesResponseType(typeof(Command), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Command>> GetCommandByIdAsync(int id)
        {
            if (id <= 0)
                return BadRequest();
            
            var command = await _commandRepository.GetCommandAsync(id);

            if (command != null)
                return Ok(command);
            
            return NotFound();
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        public async Task<ActionResult> CreateCommandAsync([FromBody]Command command)
        {
            if (command is null)
                return BadRequest();
            
            var checkCommand = await _commandRepository.GetCommandByTokenAndRequestAsync(command.Token, command.Request);

            if (checkCommand != null)
                return Conflict();
            
            await _commandRepository.CreateCommandAsync(command);
            
            return Ok();
        }
    }
}