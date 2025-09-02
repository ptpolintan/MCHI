using System.Runtime.Serialization;

namespace MHCI.Api.DTOs.Requests.Checkins
{
    [DataContract]
    public class UpdateCheckinRequestDTO
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int Mood { get; set; }

        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public string Notes { get; set; }
    }
}
