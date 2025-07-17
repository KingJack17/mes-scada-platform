using FactoryMES.Core;
using FactoryMES.Core.DTOs;
using System.Threading.Tasks;

namespace FactoryMES.Business.Interfaces
{
    public interface IAuthService
    {
        Task<User> RegisterAsync(UserRegisterDto userRegisterDto);
        Task<string> LoginAsync(UserLoginDto userLoginDto); 
    }
}