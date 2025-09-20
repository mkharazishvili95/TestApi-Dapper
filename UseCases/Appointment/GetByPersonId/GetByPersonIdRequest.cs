using TestApi.Models;

namespace TestApi.UseCases.Appointment.GetByPersonId
{
    public class GetByPersonIdRequest
    {
        public int PersonId { get; set; }
        public PaginationModel Pagination { get; set; } = new PaginationModel();
    }
}
