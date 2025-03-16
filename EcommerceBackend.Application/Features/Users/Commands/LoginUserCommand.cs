using EcommerceBackend.Application.DTOs.Users;
using EcommerceBackend.Domain.Interfaces;
using MediatR;

namespace EcommerceBackend.Application.Features.Users.Commands
{
    public class LoginUserCommand : IRequest<UserResponseDto>
    {
        public LoginUserDto LoginUserDto { get; set; }
    }

    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, UserResponseDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenService _tokenService;

        public LoginUserCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
        }

        public async Task<UserResponseDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(request.LoginUserDto.Email);
            if (user == null || !_passwordHasher.VerifyPassword(user.PasswordHash, request.LoginUserDto.Password))
            {
                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            var token = _tokenService.GenerateToken(user);

            return new UserResponseDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role,
                Token = token
            };
        }
    }
}
