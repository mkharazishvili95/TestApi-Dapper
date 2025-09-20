using Dapper;
using System.Data;
using TestApi.Enums;
using TestApi.Interfaces;
using TestApi.UseCases.Appointment.Book;
using TestApi.UseCases.Appointment.Cancel;
using TestApi.UseCases.Appointment.GetAll;
using TestApi.UseCases.Appointment.GetByDoctorId;
using TestApi.UseCases.Appointment.GetById;
using TestApi.UseCases.Appointment.GetByPersonId;

namespace TestApi.Services
{
    public class AppointmentService : IAppointmentService
    {
        readonly IDbConnection _dbConnection;
        public AppointmentService(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<BookAppointmentResponse> Book(BookAppointmentRequest request)
        {
            if (await _dbConnection.ExecuteScalarAsync<int>("SELECT COUNT(1) FROM Persons WHERE Id = @PersonId", new { request.PersonId }) == 0)
                return new BookAppointmentResponse { Success = false, StatusCode = 404, UserMessage = "Person not found." };

            if (await _dbConnection.ExecuteScalarAsync<int>("SELECT COUNT(1) FROM Doctors WHERE Id = @DoctorId", new { request.DoctorId }) == 0)
                return new BookAppointmentResponse { Success = false, StatusCode = 404, UserMessage = "Doctor not found." };

            var conflictCount = await _dbConnection.ExecuteScalarAsync<int>(
            @"SELECT COUNT(*) FROM Appointments
              WHERE DoctorId = @DoctorId
                AND AppointmentDate = @AppointmentDate
                AND Status = @ScheduledStatus",
            new { request.DoctorId, AppointmentDate = request.AppointmentDate.Date, ScheduledStatus = (int)AppointmentStatus.Scheduled });

            if(conflictCount > 0)
                return new BookAppointmentResponse { Success = false, StatusCode = 409, UserMessage = "This appointment time is already booked." };

            await _dbConnection.ExecuteAsync(
                "INSERT INTO Appointments (PersonId, DoctorId, AppointmentDate, Status) VALUES (@PersonId, @DoctorId, @AppointmentDate, @Status)",
                new { request.PersonId, request.DoctorId, request.AppointmentDate, Status = (int)AppointmentStatus.Scheduled });

            return new BookAppointmentResponse { Success = true, StatusCode = 201, UserMessage = "Appointment booked successfully." };
        }


        public async Task<CancelAppointmentResponse> Cancel(CancelAppointmentRequest request)
        {
            if (request.Id <= 0)
                return new CancelAppointmentResponse { Success = false, StatusCode = 400, UserMessage = "Id is required." };

            var appointmentQuery = @"SELECT * FROM Appointments WHERE Id = @Id";
            var appointment = await _dbConnection.QueryFirstOrDefaultAsync(appointmentQuery, new { Id = request.Id });

            if (appointment == null)
                return new CancelAppointmentResponse { Success = false, StatusCode = 404, UserMessage = "Appointment not found." };

            if (appointment.Status == (int)AppointmentStatus.Cancelled)
                return new CancelAppointmentResponse { Success = false, StatusCode = 400, UserMessage = "Appointment is already cancelled." };

            var query = @"UPDATE Appointments SET Status = @Status WHERE Id = @Id";
            var result = await _dbConnection.ExecuteAsync(query, new { Status = AppointmentStatus.Cancelled, Id = request.Id });
            if (result > 0)
                return new CancelAppointmentResponse { Success = true, StatusCode = 200, UserMessage = "Appointment cancelled successfully." };
            return new CancelAppointmentResponse { Success = false, StatusCode = 500, UserMessage = "An error occurred while cancelling the appointment." };
        }

        public async Task<GetAllAppointmentsResponse> GetAll(GetAllAppointmentsRequest request)
        {
            var query = @"
                SELECT * FROM Appointments
                /**where**/
                ORDER BY AppointmentDate
                OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY;";

            var parameters = new DynamicParameters();
            parameters.Add("Skip", request.Pagination.Skip);
            parameters.Add("Take", request.Pagination.Take);

            string whereClause = "";
            if (request.Status.HasValue)
            {
                whereClause = "WHERE Status = @Status";
                parameters.Add("Status", (int)request.Status.Value);
            }

            query = query.Replace("/**where**/", whereClause);

            var appointments = (await _dbConnection.QueryAsync<GetAllAppointmentsItemsResponse>(
                query, parameters)).ToList();

            string countSql = "SELECT COUNT(*) FROM Appointments";
            if (request.Status.HasValue)
                countSql += " WHERE Status = @Status";

            var totalCount = await _dbConnection.ExecuteScalarAsync<int>(countSql, parameters);

            return new GetAllAppointmentsResponse
            {
                Appointments = appointments,
                TotalCount = totalCount,
                Success = true,
                StatusCode = 200
            };
        }

        public async Task<GetByDoctorIdResponse> GetByDoctorId(GetByDoctorIdRequest request)
        {
            if (request.DoctorId <= 0)
                return new GetByDoctorIdResponse { Success = false, StatusCode = 400, UserMessage = "DoctorId is required." };

            var query = @"
                SELECT * FROM Appointments
                WHERE DoctorId = @DoctorId
                ORDER BY AppointmentDate
                OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY;";
            var appointments = (await _dbConnection.QueryAsync<GetByDoctorIdItemsResponse>(query,
                new { DoctorId = request.DoctorId, Skip = request.Pagination.Skip, Take = request.Pagination.Take })).ToList();

            if (!appointments.Any())
                return new GetByDoctorIdResponse { Success = false, StatusCode = 404, UserMessage = "No appointments found for the given DoctorId." };

            var totalCount = await _dbConnection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM Appointments WHERE DoctorId = @DoctorId", new { DoctorId = request.DoctorId });
            var response = new GetByDoctorIdResponse
            {
                Items = appointments,
                TotalCount = totalCount,
                Success = true,
                StatusCode = 200
            };

            return response;
        }

        public async Task<GetAppointmentByIdResponse> GetById(GetAppointmentByIdRequest request)
        {
            if (request.Id <= 0)
                return new GetAppointmentByIdResponse { Success = false, StatusCode = 400, UserMessage = "Id is required." };

            var query = @"SELECT * FROM Appointments WHERE Id = @Id";

            var appointment = await _dbConnection.QueryFirstOrDefaultAsync<GetAppointmentByIdResponse>(query, new { Id = request.Id });
            if (appointment == null)
                return new GetAppointmentByIdResponse { Success = false, StatusCode = 404, UserMessage = "Appointment not found." };

            var response = new GetAppointmentByIdResponse
            {
                Id = appointment.Id,
                PersonId = appointment.PersonId,
                DoctorId = appointment.DoctorId,
                AppointmentDate = appointment.AppointmentDate,
                Status = appointment.Status,
                StatusCode = 200,
                Success = true
            };

            return response;
        }

        public async Task<GetByPersonIdResponse> GetByPersonId(GetByPersonIdRequest request)
        {
            if (request.PersonId <= 0)
                return new GetByPersonIdResponse { Success = false, StatusCode = 400, UserMessage = "PersonId is required." };

            var query = @"
                SELECT * FROM Appointments
                WHERE PersonId = @PersonId
                ORDER BY AppointmentDate
                OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY;";

            var appointments = (await _dbConnection.QueryAsync<GetByPersonIdItemsResponse>(query,
                new { PersonId = request.PersonId, Skip = request.Pagination.Skip, Take = request.Pagination.Take })).ToList();

            if (!appointments.Any())
                return new GetByPersonIdResponse { Success = false, StatusCode = 404, UserMessage = "No appointments found for the given PersonId." };

            var totalCount = await _dbConnection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM Appointments WHERE PersonId = @PersonId", new { PersonId = request.PersonId });
            var response = new GetByPersonIdResponse
            {
                Items = appointments,
                TotalCount = totalCount,
                Success = true,
                StatusCode = 200
            };

            return response;
        }
    }
}
