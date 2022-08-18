using CarDealerAPI.Authentication;
using CarDealerAPI.Contexts;
using CarDealerAPI.DTOS;
using CarDealerAPI.Exceptions;
using CarDealerAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CarDealerAPI.Services
{
    public class AccountService : IAccountService
    {
        private readonly DealerDbContext _dealerDbContext;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly AuthenticationSettings _authenticationSettings;

        public AccountService(DealerDbContext dealerDbContext, IPasswordHasher<User> passwordHasher, AuthenticationSettings authenticationSettings)
        {
            this._dealerDbContext = dealerDbContext;
            this._passwordHasher = passwordHasher;
            this._authenticationSettings = authenticationSettings;
        }



        public string GenerateToken(UserLoginDTO login)
        {
            var user = _dealerDbContext.Users
                .Include(u => u.Role)
                .FirstOrDefault(u => u.Email == login.Email);

            if (user == null) throw new BadRequestException("Username or password is wrong");

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, login.Password);

            if (result == PasswordVerificationResult.Failed) throw new BadRequestException("Username or password is wrong");
            //refactor outside method

            List<Claim> cliams = CreateClaims(user);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));

            var credencial = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expire = DateTime.Now.AddMinutes(_authenticationSettings.JwtExpiresTime);

            var token = new JwtSecurityToken(_authenticationSettings.JwtIssuer, _authenticationSettings.JwtIssuer,
                cliams,
                expires: expire,
                signingCredentials: credencial);

            var tokenHandler = new JwtSecurityTokenHandler();

            return tokenHandler.WriteToken(token);

        }

        public void RegisterUser(UserCreateDTO userDto)
        {

            
            var newUser = new User()
            {

                DateOfBirth = userDto.DateOfBirth,
                Email = userDto.Email,
                Nationality = userDto.Nationality,
                RoleId = userDto.RoleId
                
            };
            var passwordHashed = _passwordHasher.HashPassword(newUser, userDto.Password);
            newUser.PasswordHash = passwordHashed; 
            _dealerDbContext.Add(newUser);
            _dealerDbContext.SaveChanges();
        }
        private static List<Claim> CreateClaims(User user)
        {
            var cliams = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.Role, $"{user.Role.NameRole}"),
                new Claim("DateBirth", user.DateOfBirth.Value.ToString("yyyy-MM-dd")),


            };

            if (!string.IsNullOrEmpty(user.Nationality))
            {
                cliams.Add(
                    new Claim("Nationality", user.Nationality)
                );
            }
            if (!string.IsNullOrEmpty(user.ColorEye))
            {
                cliams.Add(
                    new Claim("ColorEye", user.ColorEye)
                    );
            }

            return cliams;
        }
    }
}
