using TestApi.Enums;
using TestApi.Models;

namespace TestApi.UseCases.Appointment.GetAll
{
    public class GetAllAppointmentsResponse : BaseResponseModel
    {
        public int TotalCount { get; set; }
        public List<GetAllAppointmentsItemsResponse> Appointments { get; set; } = new();
    }
    public class GetAllAppointmentsItemsResponse
    {
        public int Id { get; set; }
        public DateTime AppointmentDate { get; set; }
        public AppointmentStatus Status { get; set; }
        public int PersonId { get; set; }
        public int DoctorId { get; set; }
    }
}
