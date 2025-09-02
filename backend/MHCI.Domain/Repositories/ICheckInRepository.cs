using MHCI.Domain.Entities;

namespace MHCI.Domain.Repositories
{
    public interface ICheckInRepository
    {
        public Task<(IEnumerable<CheckIn>, int)> GetCheckIns(int skip, int take, DateOnly? from, DateOnly? to, int? userId);
        public Task<CheckIn?> GetCheckInById(int id);
        public Task<bool> CreateCheckIn(CheckIn model);
        public Task<bool> UpdateCheckIn(CheckIn model);
    }
}
