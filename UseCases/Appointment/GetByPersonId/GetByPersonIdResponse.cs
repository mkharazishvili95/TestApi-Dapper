using TestApi.Enums;
using TestApi.Models;

namespace TestApi.UseCases.Appointment.GetByPersonId
{
    public class GetByPersonIdResponse : BaseResponseModel
    {
        public int TotalCount { get; set; }
        public List<GetByPersonIdItemsResponse> Items { get; set; } = new();
    }
    public class GetByPersonIdItemsResponse
    {
        public int? Id { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public AppointmentStatus? Status { get; set; }
        public int? DoctorId { get; set; }
    }
}
