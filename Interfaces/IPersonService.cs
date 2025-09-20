using TestApi.UseCases.Person.Create;
using TestApi.UseCases.Person.Delete;
using TestApi.UseCases.Person.GetAll;
using TestApi.UseCases.Person.GetById;

namespace TestApi.Interfaces
{
    public interface IPersonService
    {
        Task<CreatePersonResponse> Create(CreatePersonRequest request);
        Task<GetAllPersonsResponse> GetAll(GetAllPersonsRequest request);
        Task<GetPersonByIdResponse> GetById(GetPersonByIdRequest request);
        Task<DeletePersonResponse> Delete(DeletePersonRequest request);
    }
}
