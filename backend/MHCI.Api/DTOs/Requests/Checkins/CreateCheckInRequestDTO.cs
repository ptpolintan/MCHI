using System.Runtime.Serialization;

namespace MHCI.Api.DTOs.Requests.Checkins
{
    [DataContract]
    public class CreateCheckinRequestDTO
    {
        [DataMember]
        public int Mood { get; set; }

        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public string Notes { get; set; }
    }
}
