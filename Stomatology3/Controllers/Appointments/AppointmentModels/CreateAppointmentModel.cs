using System;

namespace Stomatology3.Controllers.Appointments.AppointmentmModels
{
    public class CreateAppointmentModel
    {
        public DateTime AppointmentStart { get; set; }
        //public DateTime End { get; set; }
        public int ProcedureId { get; set; }
        public string DoctorId { get; set; }
        //public User Doctor { get; set; }
        //public string PatientId { get; set; }
        //public User Patient { get; set; }

    }
}
