using TestApi.Models;

namespace TestApi.UseCases.Doctor.GetAll
{
    public class GetAllDoctorsRequest
    {
        public PaginationModel Pagination { get; set; } = new PaginationModel();
    }
}
