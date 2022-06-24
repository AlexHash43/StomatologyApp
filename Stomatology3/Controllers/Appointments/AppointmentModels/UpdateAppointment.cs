using Stomatology3.Models.Enums;
using System;

namespace Stomatology3.Controllers.Appointments.AppointmentmModels
{
    public class UpdateAppointment
    {
        public string Id { get; set; }
        public DateTime AppointmentStart { get; set; }
        //public DateTime End { get; set; }
        public int ProcedureId { get; set; }
        //public ProcedureType PrType { get; set; }
        public string DoctorId { get; set; }
        //public User Doctor { get; set; }
        public string PatientId { get; set; }
        //public User Patient { get; set; }
        public DateTime CreatedOn { get; set; }
        public AppointmentStatus Status { get; set; }
    }
}
