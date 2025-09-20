using TestApi.Enums;
using TestApi.Models;

namespace TestApi.UseCases.Appointment.GetById
{
    public class GetAppointmentByIdResponse : BaseResponseModel
    {
        public int? Id { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public AppointmentStatus? Status { get; set; }
        public int? PersonId { get; set; }
        public int? DoctorId { get; set; }
    }
}
