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
    public class UserService : IUserService  //klass som haterar användarlogik
    {
        private readonly IUserRepository _repo;                   //spara hämta användare från databas
        private readonly IMapper _mapper;                        //för att konvertera mellan user och UserDto
        private readonly IConfiguration _config;                //för att läsa värden från app.settings.json
        private readonly PasswordHasher<User> _hasher = new(); //hash/verifiera lösen

        //konstruktorn kopplar ihop klassen m dess beroenden, skickas av ASP.NET Core automatiskt när appen körs
        public UserService(IUserRepository repo, IMapper mapper, IConfiguration config) // Konstruktor med DI av repo, mapper och config
        {
            _repo = repo;
            _mapper = mapper;
            _config = config;
        }

        public async Task<bool> RegisterUserAsync(UserRegisterDto dto) //registrerar en ny användare
        {
            var existingUser = await _repo.GetByUsernameAsync(dto.Username); //kollar om användaren redan finns
            if (existingUser != null)
                return false; //om användaren redan finns ret false

            var user = new User //skapa ny user med Uname,role
            {
                Username = dto.Username,
                Role = dto.Role
            };

            user.PasswordHash = _hasher.HashPassword(user, dto.Password); //hash lösenordet

            await _repo.AddUserAsync(user); //lägg till användaren och spara i databasen
            return await _repo.SaveChangesAsync(); //returnera true om det sparades
        }

        public async Task<string?> LoginUserAsync(UserLoginDto dto) //loggar in användare och skapar en JWT-token
        {
            var user = await _repo.GetByUsernameAsync(dto.Username); //hämtar användare från databas baserat på användarnamn
            if (user == null)
                return null; //om den inte finns ret null

            //verifiera lösenord mot den hash som finns sparad
            var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if (result == PasswordVerificationResult.Failed)
                return null; //om fel lösenord = null

            var claims = new[]// Skapa claims – information som lagras i JWT-token
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };
            //skapa säkerhetsnyckel för från konfigurationen
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            //ange vilken algoritm som används för att signera token
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken( //skapa själva token
                issuer: _config["Jwt:Issuer"], //vem som utförde token
                audience: _config["Jwt:Audience"], //vem som får använda token
                claims: claims, //innehåll (användarnamn,roll)
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_config["Jwt:ExpiresInMinutes"])),//giltighetstid
                signingCredentials: creds //säkerhetssignering
            );

            return new JwtSecurityTokenHandler().WriteToken(token); //returnera token som sträng
        }
        public async Task<List<UserDTO>> GetAllUsersAsync() //hämta alla användare och returnera en lista av DTOer
        {
            var users = await _repo.GetAllAsync(); //hämta alla user objekt

            return _mapper.Map<List<UserDTO>>(users); //konvertera till userDTO och returnera

        }

    }
}
