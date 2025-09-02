using MHCI.Application.Models;

namespace MHCI.Application.Models.Responses
{
    public class GetCheckInsResponseDTO : Response<IEnumerable<CheckInModel>>
    {
        public int TotalRecords { get; set; }
    }
}
