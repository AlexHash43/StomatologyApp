using Stomatology3.Models.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Stomatology3.Models
{
    public class AppointmentModel
    {
        [Key]
        public string Id { get; set; }
        public DateTime AppointmentStart { get; set; }
        //public DateTime End { get; set; }
        [ForeignKey("ProcedureId")]
        public int ProcedureId { get; set; }
        //public ProcedureType PrType { get; set; }
        [ForeignKey("DoctorId")]
        public string DoctorId { get; set; }
        //public User Doctor { get; set; }
        [ForeignKey("PatientId")]
        public string PatientId { get; set; }
        //public User Patient { get; set; }
        public DateTime CreatedOn { get; set; }
        public AppointmentStatus Status { get; set; }

    }
}
