using MHCI.Application.Interfaces;
using MHCI.Application.Models;
using MHCI.Application.Models.Responses;
using MHCI.Application.Specification;
using MHCI.Domain.Entities;
using MHCI.Domain.Enums;
using MHCI.Domain.Repositories;
using MHCI.Infrastructure.Stores;

namespace MHCI.Application.Services
{
    public class CheckInService : ICheckInService
    {
        private readonly ICheckInRepository repository;

        public CheckInService(ICheckInRepository repository)
        {
            this.repository = repository;
        }

        public async Task<bool> CreateCheckin(CheckInModel model)
        {
            var spec = new CreateCheckInSpec();
            var errors = new List<string>();

            if (!spec.IsSatisfiedBy(model, ref errors))
                return false;

            return await this.repository.CreateCheckIn(new CheckIn { UserId = model.UserId, Mood= model.Mood, Notes = model.Notes, CreatedAt = DateOnly.ParseExact(DateTime.Now.ToString("yyyy-MM-dd"), "yyyy-MM-dd") });
        }

        public async Task<CheckInModel> GetCheckinById(int id)
        {
            var checkin = (await this.repository.GetCheckInById(id))!;
            var user = UserStore.Users.FirstOrDefault(u => u.Id == checkin.UserId)!;

            return new CheckInModel
            {
                Id = checkin.Id,
                CreatedAt = checkin.CreatedAt,
                Mood = checkin.Mood,
                Notes = checkin.Notes,
                UserId = user.Id,
                User = new UserModel
                {
                    Name = user.Name
                }
            };
        }

        public async Task<GetCheckInsResponseDTO> GetCheckins(int skip, int take, DateOnly? from, DateOnly? to, int? userId, int currentUserId)
        {
            var result = new GetCheckInsResponseDTO();

            var user = UserStore.Users.FirstOrDefault(u => u.Id == currentUserId);
            
            var currentUser = new UserModel
            {
                Id= currentUserId,
                Name = user.Name,
                Role = user.Role
            };

            if (currentUser.Role == Role.Employee)
            {
                var (checkIns, totalCount) = await repository.GetCheckIns(skip, take, from, to, currentUser.Id);
                result.TotalRecords = totalCount;

                result.Data= checkIns.Select(c => new CheckInModel
                                {
                                    Id = c.Id,
                                    CreatedAt = c.CreatedAt,
                                    Mood = c.Mood,
                                    Notes = c.Notes,
                                    UserId = c.UserId,
                                    User = currentUser
                                })
                                .ToList();
            }
            else if (currentUser.Role == Role.Manager)
            {
                var (checkIns, totalCount) = await repository.GetCheckIns(skip, take, from, to, userId);
                result.TotalRecords = totalCount;

                var users = UserStore.Users.Select(s => new UserModel
                {
                    Email = s.Email,
                    Name = s.Name,
                    Id = s.Id,
                    Role = s.Role
                });

                result.Data = checkIns.Select(c => new CheckInModel
                                {
                                    Id = c.Id,
                                    CreatedAt = c.CreatedAt,
                                    Mood = c.Mood,
                                    Notes = c.Notes,
                                    UserId = c.UserId,
                                    User = users.FirstOrDefault(u => u.Id == c.UserId)!
                                })
                                .ToList();
            }

            result.Success = true;
            return result;
        }

        public async Task<bool> UpdateCheckin(CheckInModel model)
        {
            var spec = new UpdateCheckInSpec();
            var errors = new List<string>();

            if (!spec.IsSatisfiedBy(model, ref errors))
                return false;

            await this.repository.UpdateCheckIn(new CheckIn { Id = model.Id, UserId = model.UserId, Mood = model.Mood, Notes = model.Notes });

            return true;
        }
    }
}
