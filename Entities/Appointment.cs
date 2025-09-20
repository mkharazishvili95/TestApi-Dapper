using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TestApi.Enums;

namespace TestApi.Entities
{
    public class Appointment
    {
        [Key]
        public int Id { get;set; }
        public DateTime AppointmentDate { get; set; }
        public AppointmentStatus Status { get; set; }
        [ForeignKey("PersonId")]
        public int PersonId { get; set; }
        public Person Person { get; set; }
        [ForeignKey("DoctorId")]
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }
    }
}
