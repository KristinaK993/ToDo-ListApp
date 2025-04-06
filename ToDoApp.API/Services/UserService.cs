using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ToDoApp.API.Models.DTOs;
using ToDoApp.API.Models.Entities;
using ToDoApp.API.Repositories;


namespace ToDoApp.API.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly PasswordHasher<User> _hasher = new();

        public UserService(IUserRepository repo, IMapper mapper, IConfiguration config)
        {
            _repo = repo;
            _mapper = mapper;
            _config = config;
        }

        public async Task<bool> RegisterUserAsync(UserRegisterDto dto)
        {
            var existingUser = await _repo.GetByUsernameAsync(dto.Username);
            if (existingUser != null)
                return false;

            var user = new User
            {
                Username = dto.Username,
                PasswordHash = _hasher.HashPassword(null, dto.Password),
                Role = dto.Role
            };

            user.PasswordHash = _hasher.HashPassword(user, dto.Password);

            await _repo.AddUserAsync(user);
            return await _repo.SaveChangesAsync();
        }

        public async Task<string?> LoginUserAsync(UserLoginDto dto)
        {
            var user = await _repo.GetByUsernameAsync(dto.Username);
            if (user == null)
                return null;

            var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if (result == PasswordVerificationResult.Failed)
                return null;

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_config["Jwt:ExpiresInMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public async Task<List<UserDTO>> GetAllUsersAsync()
        {
            var users = await _repo.GetAllAsync();

            return _mapper.Map<List<UserDTO>>(users);

        }

    }
}
