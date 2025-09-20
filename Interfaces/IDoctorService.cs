using TestApi.UseCases.Doctor.Create;
using TestApi.UseCases.Doctor.Delete;
using TestApi.UseCases.Doctor.GetAll;
using TestApi.UseCases.Doctor.GetById;

namespace TestApi.Interfaces
{
    public interface IDoctorService
    {
        Task<CreateDoctorResponse> Create(CreateDoctorRequest request);
        Task<GetAllDoctorsResponse> GetAll(GetAllDoctorsRequest request);
        Task<GetDoctorByIdResponse> GetById(GetDoctorByIdRequest request);
        Task<DeleteDoctorResponse> Delete(DeleteDoctorRequest request);
    }
}
