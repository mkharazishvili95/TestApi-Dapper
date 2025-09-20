using TestApi.Enums;
using TestApi.Models;

namespace TestApi.UseCases.Appointment.GetAll
{
    public class GetAllAppointmentsRequest
    {
        public AppointmentStatus? Status { get; set; }
        public PaginationModel Pagination { get; set; } = new PaginationModel();
    }
}
