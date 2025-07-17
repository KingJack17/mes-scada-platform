using System.Collections.Generic;

namespace FactoryMES.Core.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Email { get; set; }
        public int SicilNo { get; set; }
        public bool IsActive { get; set; }
        public List<string> Roles { get; set; }
    }
}