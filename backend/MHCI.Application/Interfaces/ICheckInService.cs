using MHCI.Application.Models;
using MHCI.Application.Models.Responses;

namespace MHCI.Application.Interfaces
{
    public interface ICheckInService
    {
        public Task<GetCheckInsResponseDTO> GetCheckins(int skip, int take, DateOnly? from, DateOnly? to, int? userId, int currentUserId);
        public Task<CheckInModel> GetCheckinById(int id);
        public Task<bool> CreateCheckin(CheckInModel model);
        public Task<bool> UpdateCheckin(CheckInModel model);
    }
}
