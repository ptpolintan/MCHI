namespace MHCI.Application.Models
{
    public class CheckInModel
    {
        public int Id { get; set; }
        public int Mood { get; set; }
        public string Notes { get; set; }
        public DateOnly CreatedAt { get; set; }

        public int UserId { get; set; }
        public UserModel User { get; set; }
    }
}
