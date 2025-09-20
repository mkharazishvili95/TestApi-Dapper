using TestApi.Models;

namespace TestApi.UseCases.Person.GetAll
{
    public class GetAllPersonsRequest
    {
        public PaginationModel Pagination { get; set; } = new PaginationModel();
    }
}
