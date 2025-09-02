using MHCI.Domain.Entities;
using MHCI.Domain.Enums;

namespace MHCI.Infrastructure.Stores
{
    public static class UserStore
    {
        public static IReadOnlyCollection<User> Users { get; } =
        [
            new User { Id = 1, Name = "Alice", Email = "alice@example.com", Password = "1234", Role = Role.Employee },
            new User { Id = 2, Name = "Bob", Email = "bob@example.com", Password = "1234", Role = Role.Employee },
            new User { Id = 3, Name = "Charlie", Email = "charlie@example.com", Password = "1234", Role = Role.Manager },
        ];
    }
}
