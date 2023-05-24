using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Application.DTO;
using Restaurant.Application.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Restaurant.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IValidator<RegisterUserDto> _validator;

        public AccountController(IAccountService accountService,IValidator<RegisterUserDto> validator)
        {
           _accountService = accountService;
          _validator = validator;
        }

        [HttpPost("Register")]
        public async Task<ActionResult> RegisterUser([FromBody] RegisterUserDto userDto)
        {
            var validation = await  _validator.ValidateAsync(userDto);
            if (!validation.IsValid)
            {
                return BadRequest(validation.Errors);
            }
            _accountService.RegisterUser(userDto);
            return Ok();
        }
        [HttpPost("Login")]
        public ActionResult Login([FromBody] LoginDto dto)
        {
            string token = _accountService.GenerateJwt(dto);
            return Ok(token);
        }
    }
}
