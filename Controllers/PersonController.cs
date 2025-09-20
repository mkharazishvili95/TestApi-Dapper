using Microsoft.AspNetCore.Mvc;
using TestApi.Interfaces;
using TestApi.UseCases.Person.Create;
using TestApi.UseCases.Person.Delete;
using TestApi.UseCases.Person.GetAll;
using TestApi.UseCases.Person.GetById;

namespace TestApi.Controllers
{
    [ApiController]
    [Route("api/person")]
    public class PersonController : ControllerBase
    {
        readonly IPersonService _personService;
        public PersonController(IPersonService personService)
        {
            _personService = personService;
        }

        [HttpPost("create")]
        public async Task<CreatePersonResponse> Create([FromBody] CreatePersonRequest request) => await _personService.Create(request);

        [HttpPost("all")]
        public async Task<GetAllPersonsResponse> GetAll(GetAllPersonsRequest request) => await _personService.GetAll(request);

        [HttpGet("id")]
        public async Task<GetPersonByIdResponse> GetById([FromQuery] GetPersonByIdRequest request) =>  await _personService.GetById(request);

        [HttpDelete("id")]
        public async Task<DeletePersonResponse> Delete([FromQuery] DeletePersonRequest request) => await _personService.Delete(request);
    }
}
