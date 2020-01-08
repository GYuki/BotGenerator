using BotService.API.Model;
using BotService.API.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;

namespace BotService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly IUserRepository _userRepository;
        
        
        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        [Route("{id:int}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<User>> UserByIdAsync(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var user = await _userRepository.GetUserAsync(id);

            if (user != null)
                return user;
            
            return NotFound();
        }

        //POST api/v1/[controller]/users
        // [Route("users")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> CreateUserAsync([FromBody]User user)
        {
            if (user is null)
                return BadRequest();
            
            await _userRepository.CreateUserAsync(user);

            return Ok();
        }
    }
}