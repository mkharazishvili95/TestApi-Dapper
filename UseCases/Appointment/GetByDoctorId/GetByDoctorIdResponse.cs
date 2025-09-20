using TestApi.Enums;
using TestApi.Models;

namespace TestApi.UseCases.Appointment.GetByDoctorId
{
    public class GetByDoctorIdResponse : BaseResponseModel
    {
        public int TotalCount { get; set; }
        public List<GetByDoctorIdItemsResponse> Items { get; set; } = new();
    }
    public class GetByDoctorIdItemsResponse
    {
        public int? Id { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public AppointmentStatus? Status { get; set; }
        public int? PersonId { get; set; }
    }
}
