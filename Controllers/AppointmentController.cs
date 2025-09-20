using Microsoft.AspNetCore.Mvc;
using TestApi.Interfaces;
using TestApi.UseCases.Appointment.Book;
using TestApi.UseCases.Appointment.Cancel;
using TestApi.UseCases.Appointment.GetAll;
using TestApi.UseCases.Appointment.GetByDoctorId;
using TestApi.UseCases.Appointment.GetById;
using TestApi.UseCases.Appointment.GetByPersonId;

namespace TestApi.Controllers
{
    [ApiController]
    [Route("api/apointment")]
    public class AppointmentController : ControllerBase
    {
        readonly IAppointmentService _appointmentService;
        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpPost("book")]
        public async Task<BookAppointmentResponse> Book([FromBody] BookAppointmentRequest request) => await _appointmentService.Book(request);

        [HttpPost("all")]
        public async Task<GetAllAppointmentsResponse> GetAll(GetAllAppointmentsRequest request) => await _appointmentService.GetAll(request);

        [HttpGet("doctor-id")]
        public async Task<GetByDoctorIdResponse> GetByDoctorId([FromBody] GetByDoctorIdRequest request) => await _appointmentService.GetByDoctorId(request);

        [HttpGet("person-id")]
        public async Task<GetByPersonIdResponse> GetByPersonId([FromBody] GetByPersonIdRequest request) => await _appointmentService.GetByPersonId(request);

        [HttpGet("id")]
        public async Task<GetAppointmentByIdResponse> GetById([FromQuery] GetAppointmentByIdRequest request) => await _appointmentService.GetById(request);

        [HttpDelete("cancel")]
        public async Task<CancelAppointmentResponse> Cancel([FromQuery] CancelAppointmentRequest request) => await _appointmentService.Cancel(request);
    }
}
