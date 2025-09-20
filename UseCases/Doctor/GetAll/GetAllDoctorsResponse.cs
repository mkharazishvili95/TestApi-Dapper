using TestApi.Models;

namespace TestApi.UseCases.Doctor.GetAll
{
    public class GetAllDoctorsResponse : BaseResponseModel
    {
        public int TotalCount { get; set; }
        public List<GetAllDoctorsItemsResponse> Items { get; set; } = new();
    }
    public class GetAllDoctorsItemsResponse
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
