using FactoryMES.Business.Interfaces;
using FactoryMES.Core.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FactoryMES.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto userRegisterDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var createdUser = await _authService.RegisterAsync(userRegisterDto);
                // Başarılı kayıt sonrası şifre gibi hassas verileri asla geri dönmüyoruz.
                return Ok(new { message = "Kullanıcı başarıyla oluşturuldu.", userId = createdUser.Id });
            }
            catch (Exception ex)
            {
                // Servisten gelen "Bu kullanıcı adı zaten alınmış." gibi hataları yakalıyoruz.
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto userLoginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var jwtToken = await _authService.LoginAsync(userLoginDto);
                return Ok(new { token = jwtToken });
            }
            catch (Exception ex)
            {
                // Servisten gelen "Geçersiz kullanıcı adı veya şifre" hatasını yakalıyoruz.
                // 401 Unauthorized, hatalı giriş denemeleri için standart yanıttır.
                return Unauthorized(ex.Message);
            }
        }
    }
}