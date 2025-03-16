using EcommerceBackend.Application.DTOs.Users;
using EcommerceBackend.Application.Features.Users.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceBackend.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserResponseDto>> Register(RegisterUserDto registerUserDto)
        {
            var command = new RegisterUserCommand { RegisterUserDto = registerUserDto };
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserResponseDto>> Login(LoginUserDto loginUserDto)
        {
            var command = new LoginUserCommand { LoginUserDto = loginUserDto };
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
