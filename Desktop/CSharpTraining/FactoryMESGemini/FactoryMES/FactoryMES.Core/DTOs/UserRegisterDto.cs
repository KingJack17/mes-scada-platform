using System.ComponentModel.DataAnnotations;

namespace FactoryMES.Core.DTOs
{
    public class UserRegisterDto
    {
        [Required(ErrorMessage = "Kullanıcı adı zorunludur.")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Şifre zorunludur.")]
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        
        public int SicilNo { get; set; }
        [EmailAddress(ErrorMessage = "Lütfen geçerli bir e-posta adresi girin.")]
        public string? Email { get; set; }
    }
}