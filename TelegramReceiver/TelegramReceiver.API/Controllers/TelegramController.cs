using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TelegramReceiver.API.Models;
using TelegramReceiver.API.Infrastructure.Repositories;

namespace TelegramReceiver.API.Controllers
{
    [Route("receiver/[controller]")]
    [ApiController]
    public class TelegramController : Controller
    {
        private readonly ICommandRepository _commandRepository;

        public TelegramController(ICommandRepository commandRepository)
        {
            _commandRepository = commandRepository;
        }

        [HttpPost]
        [Route("{botToken}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> HandleMessageAsync(string botToken, [FromBody]Message message)
        {
            await Task.FromResult(0); // for debugs
            return Ok();
        }
    }
}