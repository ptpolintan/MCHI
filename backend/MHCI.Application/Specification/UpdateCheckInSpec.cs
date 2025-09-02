using MHCI.Application.Models;
namespace MHCI.Application.Specification
{
    public class UpdateCheckInSpec : ISpecification<CheckInModel>
    {
        public bool IsSatisfiedBy(CheckInModel entity, ref List<string> error)
        {
            if (entity.Id <= 0)
            {
                error.Add("Invalid CheckIn Id.");
            }

            if (entity.UserId <= 0)
            {
                error.Add("Invalid UserId.");
            }

            if (entity.Mood < 1 || entity.Mood > 5)
            {
                error.Add("Mood must be between 1 and 5.");
            }

            if (!string.IsNullOrEmpty(entity.Notes) && entity.Notes.Length > 500)
            {
                error.Add("Notes cannot exceed 500 characters.");
            }

            return error.Count == 0;
        }
    }
}
