using EcommerceBackend.Application.DTOs.Users;
using EcommerceBackend.Domain.Interfaces;
using EcommerceBackend.Domain.Users;
using MediatR;

namespace EcommerceBackend.Application.Features.Users.Commands
{
    public class RegisterUserCommand : IRequest<UserResponseDto>
    {
        public RegisterUserDto RegisterUserDto { get; set; }
    }

    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, UserResponseDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenService _tokenService;

        public RegisterUserCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
        }

        public async Task<UserResponseDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var user = new User
            {
                Username = request.RegisterUserDto.Username,
                Email = request.RegisterUserDto.Email,
                PasswordHash = _passwordHasher.HashPassword(request.RegisterUserDto.Password),
                Role = request.RegisterUserDto.Role
            };

            await _userRepository.AddAsync(user);

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
