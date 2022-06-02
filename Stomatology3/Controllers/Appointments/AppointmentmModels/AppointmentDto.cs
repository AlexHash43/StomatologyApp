using Stomatology3.Models.Enums;
using System;

namespace Stomatology3.Controllers.Appointments.AppointmentmModels
{
    public class AppointmentDto
    {
        //public Guid Id { get; set; }
        public DateTime AppointmentStart { get; set; }
        //public DateTime End { get; set; }
        public string ProcedureName { get; set; }
        public string DoctorFullName { get; set; }
        //public User Doctor { get; set; }
        public string PatientFullName { get; set; }
        //public User Patient { get; set; }
        public DateTime CreatedOn { get; set; }
        public AppointmentStatus Status { get; set; }
    }
}
