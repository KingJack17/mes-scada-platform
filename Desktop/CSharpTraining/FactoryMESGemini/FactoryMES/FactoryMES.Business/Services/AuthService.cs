using FactoryMES.Business.Interfaces;
using FactoryMES.Core;
using FactoryMES.Core.DTOs;
using FactoryMES.Core.Interfaces;
using Microsoft.EntityFrameworkCore; // Include ve ThenInclude için eklendi
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FactoryMES.Business.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public AuthService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public async Task<string> LoginAsync(UserLoginDto userLoginDto)
        {
            // Kullanıcıyı, rolleriyle birlikte veritabanından çekiyoruz.
            var user = await _unitOfWork.Users.GetQueryable() // Önce sorgulanabilir bir nesne alıyoruz
                                              .Include(u => u.UserRoles) // UserRoles ilişkisini dahil et
                                              .ThenInclude(ur => ur.Role) // UserRoles'dan Role'e geçiş yap ve onu da dahil et
                                              .FirstOrDefaultAsync(u => u.Username.ToLower() == userLoginDto.Username.ToLower()); // Sonunda sorguyu çalıştır

            // Kullanıcı bulunamazsa veya şifre yanlışsa hata fırlat
            if (user == null || !BCrypt.Net.BCrypt.Verify(userLoginDto.Password, user.PasswordHash))
            {
                throw new Exception("Geçersiz kullanıcı adı veya şifre.");
            }

            // Kullanıcının rollerini isim olarak bir listeye alıyoruz.
            var roles = user.UserRoles.Select(ur => ur.Role.Name).ToList();

            // Token oluşturma fonksiyonuna kullanıcıyı ve rollerini gönderiyoruz.
            return GenerateJwtToken(user, roles);
        }

        public async Task<User> RegisterAsync(UserRegisterDto userRegisterDto)
        {
            var existingUser = (await _unitOfWork.Users.FindAsync(u => u.Username.ToLower() == userRegisterDto.Username.ToLower())).FirstOrDefault();
            if (existingUser != null)
            {
                throw new Exception("Bu kullanıcı adı zaten alınmış.");
            }

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(userRegisterDto.Password);

            var user = new User
            {
                Username = userRegisterDto.Username,
                PasswordHash = passwordHash,
                FirstName = userRegisterDto.FirstName,
                LastName = userRegisterDto.LastName,
                Email = userRegisterDto.Email,
                SicilNo = userRegisterDto.SicilNo,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.CompleteAsync();

            return user;
        }

        private string GenerateJwtToken(User user, List<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // Her bir rol için ayrı bir 'role' claim'i ekliyoruz.
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            var expires = DateTime.UtcNow.AddHours(8);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}