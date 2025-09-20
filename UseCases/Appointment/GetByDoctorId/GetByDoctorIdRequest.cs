using TestApi.Models;

namespace TestApi.UseCases.Appointment.GetByDoctorId
{
    public class GetByDoctorIdRequest
    {
        public int DoctorId { get; set; }
        public PaginationModel Pagination { get; set; } = new PaginationModel();
    }
}
