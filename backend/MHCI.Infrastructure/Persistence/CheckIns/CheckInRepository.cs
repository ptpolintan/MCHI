using MHCI.Domain.Entities;
using MHCI.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MHCI.Infrastructure.Persistence.CheckIns
{
    public class CheckInRepository : ICheckInRepository
    {
        public readonly AppDbContext context;

        public CheckInRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<bool> CreateCheckIn(CheckIn model)
        {
            try
            {
                context.Checkins.Add(model);
                return await context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<CheckIn?> GetCheckInById(int id)
        {
            return await context.Checkins.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<(IEnumerable<CheckIn>, int)> GetCheckIns(int skip, int take, DateOnly? from, DateOnly? to, int? userId)
        {
            var query = context.Checkins.AsQueryable();

            if (userId.HasValue)
                query = query.Where(c => c.UserId == userId.Value);

            // Optional filtering by date range
            if (from.HasValue)
                query = query.Where(c => c.CreatedAt >= from.Value);
            if (to.HasValue)
                query = query.Where(c => c.CreatedAt <= to.Value);

            var count = (await query
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync()).Count;

            var result = await query
                .OrderByDescending(c => c.CreatedAt)
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            // Order by date descending, apply pagination
            return (result, count);
        }

        public async Task<bool> UpdateCheckIn(CheckIn model)
        {
            context.Checkins.Update(model);
            return await context.SaveChangesAsync() > 0;
        }
    }
}
