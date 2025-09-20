using TestApi.Models;

namespace TestApi.UseCases.Person.GetAll
{
    public class GetAllPersonsResponse : BaseResponseModel
    {
        public int TotalCount { get; set; }
        public List<GetAllPersonsItemsResponse> Items { get; set; } = new();
    }
    public class GetAllPersonsItemsResponse 
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
