using MHCI.Domain.Enums;

namespace MHCI.Application.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }
    }
}
