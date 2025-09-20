using Dapper;
using System.Data;
using TestApi.Interfaces;
using TestApi.UseCases.Person.Create;
using TestApi.UseCases.Person.Delete;
using TestApi.UseCases.Person.GetAll;
using TestApi.UseCases.Person.GetById;
using TestApi.Validation.Person;

namespace TestApi.Services
{
    public class PersonService : IPersonService
    {
        readonly IDbConnection _dbConnection;
        public PersonService(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }
        public async Task<CreatePersonResponse> Create(CreatePersonRequest request)
        {
            var validator = new PersonValidation();
            var validationResult = await validator.ValidateAsync(request);
            if(!validationResult.IsValid)
                return new CreatePersonResponse { Success = false, StatusCode = 400, UserMessage = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)) };

            var query = @$"INSERT INTO Persons (FirstName,LastName,DateOfBirth)
                        VALUES(@FirstName, @LastName, @DateOfBirth)";

            var result = await _dbConnection.ExecuteAsync(query, new { request.FirstName, request.LastName, request.DateOfBirth });

            return result > 0 ? new CreatePersonResponse { Success = true, StatusCode = 201, UserMessage = "Person created successfully." }
                : new CreatePersonResponse { Success = false, StatusCode = 500, UserMessage = "An error occurred while creating the person." };
        }

        public async Task<GetPersonByIdResponse> GetById(GetPersonByIdRequest request)
        {
            if (request.Id <= 0)
                return new GetPersonByIdResponse { Success = false, StatusCode = 400, UserMessage = "Invalid Id" };

            var sql = @$"SELECT Id, FirstName, LastName
                     FROM Persons
                     WHERE Id = {request.Id}";


            var person = await _dbConnection.QueryFirstOrDefaultAsync<GetPersonByIdResponse>(
                sql,
                new { Id = request.Id }
            );

            return person == null ? new GetPersonByIdResponse() { UserMessage = "Person not found." }
                :
                new GetPersonByIdResponse
                {
                    Id = person.Id,
                    FirstName = person.FirstName,
                    LastName = person.LastName,
                    DateOfBirth = person.DateOfBirth,
                    StatusCode = 200,
                    Success = true
                };
        }

        public async Task<GetAllPersonsResponse> GetAll(GetAllPersonsRequest request)
        {
            var query = @"
                SELECT * FROM Persons
                ORDER BY Id
                OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY";

            var persons = (await _dbConnection.QueryAsync<GetAllPersonsItemsResponse>(
                query,
                new { Skip = request.Pagination.Skip, Take = request.Pagination.Take }
            )).ToList();

            var totalCount = await _dbConnection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM Persons");

            if (!persons.Any())
                return new GetAllPersonsResponse() { UserMessage = "Persons not found." };


            return new GetAllPersonsResponse
            {
                TotalCount = totalCount,
                Success = true,
                Items = persons,
                StatusCode = 200
            };
        }

        public async Task<DeletePersonResponse> Delete(DeletePersonRequest request)
        {
            if (request.Id <= 0)
                return new DeletePersonResponse { Success = false, StatusCode = 400, UserMessage = "Id is required." };


            var personQuery = @"SELECT * FROM Persons WHERE Id = @Id";
            var person = await _dbConnection.QueryFirstOrDefaultAsync<GetPersonByIdResponse>(personQuery, new { Id = request.Id });

            if (person == null)
                return new DeletePersonResponse { Success = false, StatusCode = 404, UserMessage = "Person not found." };

            var query = @"DELETE FROM Persons WHERE Id = @Id";
            var result = await _dbConnection.ExecuteAsync(query, new { Id = request.Id });
            return result > 0 ? new DeletePersonResponse { Success = true, StatusCode = 200, UserMessage = "Person deleted successfully." }
                : new DeletePersonResponse { Success = false, StatusCode = 500, UserMessage = "An error occurred while deleting the person." };
        }
    }
}
