using MHCI.Application.Models;

namespace MHCI.Api.DTOs.Responses
{
    public class GetCheckInsResponseDTO : Response<IEnumerable<CheckInModel>>
    {
        public int TotalRecords { get; set; }
    }
}
