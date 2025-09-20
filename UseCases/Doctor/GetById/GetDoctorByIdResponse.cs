using TestApi.Models;

namespace TestApi.UseCases.Doctor.GetById
{
    public class GetDoctorByIdResponse : BaseResponseModel
    {
        public int? Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}
