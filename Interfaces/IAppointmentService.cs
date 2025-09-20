using TestApi.Services;
using TestApi.UseCases.Appointment.Book;
using TestApi.UseCases.Appointment.Cancel;
using TestApi.UseCases.Appointment.GetAll;
using TestApi.UseCases.Appointment.GetByDoctorId;
using TestApi.UseCases.Appointment.GetById;
using TestApi.UseCases.Appointment.GetByPersonId;

namespace TestApi.Interfaces
{
    public interface IAppointmentService
    {
        Task<BookAppointmentResponse> Book(BookAppointmentRequest request);
        Task<GetByDoctorIdResponse> GetByDoctorId(GetByDoctorIdRequest request);
        Task<GetByPersonIdResponse> GetByPersonId(GetByPersonIdRequest request);
        Task<GetAllAppointmentsResponse> GetAll(GetAllAppointmentsRequest request);
        Task<GetAppointmentByIdResponse> GetById(GetAppointmentByIdRequest request);
        Task<CancelAppointmentResponse> Cancel(CancelAppointmentRequest request);
    }
}
