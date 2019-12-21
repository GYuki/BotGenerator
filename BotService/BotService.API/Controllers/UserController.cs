using BotService.API.Model;
using BotService.API.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;

namespace BotService.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly BotContext _botContext;
        
        
        public UserController(BotContext context)
        {
            _botContext = context ?? throw new ArgumentException(nameof(context));

            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        [HttpGet]
        [Route("users/{id:int}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<User>> UserByIdAsync(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var user = await _botContext.Users.SingleOrDefaultAsync(u => u.Id == id);

            if (user != null)
                return user;
            
            return NotFound();
        }

        //POST api/v1/[controller]/users
        [Route("users")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> CreateUserAsync([FromBody]User user)
        {
            var _user = new User
            {
                SenderId = user.SenderId
            };

            _botContext.Users.Add(_user);

            await _botContext.SaveChangesAsync();

            return CreatedAtAction(nameof(UserByIdAsync), new { id = _user.Id }, null);
        }
    }
}