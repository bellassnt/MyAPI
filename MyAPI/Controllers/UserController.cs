using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyAPI.AuthorizationAndAuthentication;
using MyAPI.Dtos;
using MyAPI.Enums;
using MyAPI.Interfaces;
using MyAPI.Models;

namespace MyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _repository;
        private readonly IConfiguration _configuration;
        private readonly TokenGenerator _tokenGenerator;

        public UserController(IUserRepository repository, IConfiguration configuration, TokenGenerator tokenGenerator)
        {
            _repository = repository;
            _configuration = configuration;
            _tokenGenerator = tokenGenerator;
        }

        [HttpGet]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Get([FromQuery] int page, int maxResultsPerPage)
        {
            var users = await _repository.Get(page, maxResultsPerPage);

            return Ok(users);
        }

        [HttpPost]
        [Route("login/internal")]
        [AllowAnonymous]
        public async Task<IActionResult> InternalLogin([FromBody] UserpassDto userpassDto)
        {
            var user = await _repository.Get(userpassDto.Login!, userpassDto.Password!);

            if (user == null)
                return NotFound(new { message = "We could not find this user or the password is invalid." });

            var token = _tokenGenerator.GenerateToken(user);

            return Ok(new { token });
        }


        [HttpPost]
        [Route("login/external")]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLogin([FromBody] UserpassDto userpassDto)
        {
            var auth = _configuration.GetSection("Authentication");

            if (userpassDto.Login != auth.GetValue<string>("login")
                || userpassDto.Password != auth.GetValue<string>("password"))
                return NotFound(new { message = "We could not find this user or the password is invalid." });

            var user = new User()
            {
                Role = UserRole.Manager,
            };

            var token = _tokenGenerator.GenerateToken(user);

            return Ok(new { token });
        }
    }
}