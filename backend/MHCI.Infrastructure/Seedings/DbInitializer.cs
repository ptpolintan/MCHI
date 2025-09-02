using MHCI.Domain.Entities;
using MHCI.Infrastructure.Persistence;
using MHCI.Infrastructure.Stores;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHCI.Infrastructure.Seedings
{
    public static class DbInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            context.Database.Migrate();

            var users = UserStore.Users;

            if (!context.Checkins.Any())
            {
                var checkIns = new List<CheckIn>();

                foreach (var user in users)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        checkIns.Add(new CheckIn
                        {
                            UserId = user.Id,
                            Mood = Random.Shared.Next(1, 6), // mood 1-5
                            Notes = $"[SEEDED] Check-in {i + 1} for {user.Id}",
                            CreatedAt = DateOnly.FromDateTime(DateTime.Now.AddDays(-i))
                        });
                    }
                }

                context.Checkins.AddRange(checkIns);
                context.SaveChanges();
            }
        }
    }
}
