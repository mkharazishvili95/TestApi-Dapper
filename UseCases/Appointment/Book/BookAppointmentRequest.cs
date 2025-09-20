namespace TestApi.UseCases.Appointment.Book
{
    public class BookAppointmentRequest
    {
        public int PersonId { get; set; }
        public int DoctorId { get; set; }
        public DateTime AppointmentDate { get; set; }
    }
}
