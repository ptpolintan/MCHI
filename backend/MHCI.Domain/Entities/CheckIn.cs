namespace MHCI.Domain.Entities
{
    public class CheckIn
    {
        public int Id { get; set; }
        public int Mood { get; set; }
        public string Notes { get; set; }
        public DateOnly CreatedAt { get; set; }

        public int UserId { get; set; }
    }
}
