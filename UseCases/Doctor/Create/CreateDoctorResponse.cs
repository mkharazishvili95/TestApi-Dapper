using TestApi.Models;

namespace TestApi.UseCases.Doctor.Create
{
    public class CreateDoctorResponse : BaseResponseModel
    {
        public int? DoctorId { get; set; }
    }
}
