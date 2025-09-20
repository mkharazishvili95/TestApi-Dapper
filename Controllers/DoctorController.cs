using Microsoft.AspNetCore.Mvc;
using TestApi.Interfaces;
using TestApi.UseCases.Doctor.Create;
using TestApi.UseCases.Doctor.Delete;
using TestApi.UseCases.Doctor.GetAll;
using TestApi.UseCases.Doctor.GetById;

namespace TestApi.Controllers
{
    [ApiController]
    [Route("api/doctor")]
    public class DoctorController : ControllerBase
    {
        readonly IDoctorService _doctorService;
        public DoctorController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        [HttpPost("create")]
        public async Task<CreateDoctorResponse> Create([FromBody] CreateDoctorRequest request) => await _doctorService.Create(request);

        [HttpPost("all")]
        public async Task<GetAllDoctorsResponse> GetAll(GetAllDoctorsRequest request) => await _doctorService.GetAll(request);

        [HttpGet("id")]
        public async Task<GetDoctorByIdResponse> GetById([FromQuery] GetDoctorByIdRequest request) => await _doctorService.GetById(request);

        [HttpDelete]
        public async Task<DeleteDoctorResponse> Delete([FromQuery] DeleteDoctorRequest request) => await _doctorService.Delete(request);
    }
}
