using TestApi.Models;

namespace TestApi.UseCases.Person.GetById
{
    public class GetPersonByIdResponse : BaseResponseModel
    {
        public int? Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }
}
