using Dapper;
using System.Data;
using TestApi.Entities;
using TestApi.Interfaces;
using TestApi.UseCases.Doctor.Create;
using TestApi.UseCases.Doctor.Delete;
using TestApi.UseCases.Doctor.GetAll;
using TestApi.UseCases.Doctor.GetById;
using TestApi.UseCases.Person.GetAll;
using TestApi.Validation.Doctor;

namespace TestApi.Services
{
    public class DoctorService : IDoctorService
    {
        readonly IDbConnection _dbConnection;
        public DoctorService(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<CreateDoctorResponse> Create(CreateDoctorRequest request)
        {
            var validator = new DoctorValidation();
            var validationResult = await validator.ValidateAsync(request);

            if(!validationResult.IsValid)
                return new CreateDoctorResponse { Success = false, StatusCode = 400, UserMessage = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)) };

            var query = @"INSERT INTO Doctors(FirstName, LastName)
                        VALUES(@FirstName, @LastName)";

            var result = await _dbConnection.ExecuteAsync(query, new { FirstName = request.FirstName, LastName = request.LastName });

            if(result > 0)
                return new CreateDoctorResponse { Success = true, StatusCode = 201, UserMessage = "Doctor created successfully." };

            return new CreateDoctorResponse { Success = false, StatusCode = 500, UserMessage = "An error occurred while creating the doctor." };
        }

        public async Task<DeleteDoctorResponse> Delete(DeleteDoctorRequest request)
        {
            if(request.Id <= 0)
                return new DeleteDoctorResponse { Success = false, StatusCode = 400, UserMessage = "Id is required." };

            var personQuery = @"SELECT * FROM Doctors WHERE Id = @Id";
            var person = await _dbConnection.QueryFirstOrDefaultAsync<Doctor>(personQuery, new { Id = request.Id });

            if (person == null)
                return new DeleteDoctorResponse { Success = false, StatusCode = 404, UserMessage = "Doctor not found." };

            var query = @"DELETE FROM Doctors WHERE Id = @Id";
            var result = await _dbConnection.ExecuteAsync(query, new { Id = request.Id });

            if(result > 0)
                return new DeleteDoctorResponse { Success = true, StatusCode = 200, UserMessage = "Doctor deleted successfully." };
            return new DeleteDoctorResponse { Success = false, StatusCode = 500, UserMessage = "An error occurred while deleting the doctor." };
        }

        public async Task<GetAllDoctorsResponse> GetAll(GetAllDoctorsRequest request)
        {
            var query = @"
                SELECT * FROM Doctors
                ORDER BY Id
                OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY";

            var doctors = (await _dbConnection.QueryAsync<GetAllDoctorsItemsResponse>(
                query,
                new { Skip = request.Pagination.Skip, Take = request.Pagination.Take }
            )).ToList();

            var totalCount = await _dbConnection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM Doctors");

            if (!doctors.Any())
                return new GetAllDoctorsResponse { Success = true, StatusCode = 200, UserMessage = "No doctors found.", TotalCount = 0, Items = new List<GetAllDoctorsItemsResponse>() };

            return new GetAllDoctorsResponse { Success = true, StatusCode = 200, TotalCount = totalCount, Items = doctors };
        }

        public async Task<GetDoctorByIdResponse> GetById(GetDoctorByIdRequest request)
        {
            if(request.Id <= 0)
                return new GetDoctorByIdResponse { Success = false, StatusCode = 400, UserMessage = "Id is required." };

            var query = @"SELECT * FROM Doctors WHERE Id = @Id";

            var person = await _dbConnection.QueryFirstOrDefaultAsync<GetDoctorByIdResponse>(query, new { Id = request.Id });

            if(person == null)
                return new GetDoctorByIdResponse { Success = false, StatusCode = 404, UserMessage = "Doctor not found." };

            var response = new GetDoctorByIdResponse
            {
                Id = person.Id,
                FirstName = person.FirstName,
                LastName = person.LastName,
                StatusCode = 200,
                Success = true
            };

            return response;
        }
    }
}
