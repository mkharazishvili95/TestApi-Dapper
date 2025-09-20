using TestApi.Models;

namespace TestApi.UseCases.Person.Create
{
    public class CreatePersonResponse : BaseResponseModel
    {
        public int? PersonId { get; set; }
    }
}
