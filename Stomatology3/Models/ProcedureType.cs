using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Stomatology3.Models
{
    public class ProcedureType
    {
        [Required, Key]
        public int ProcId { get; set; }
        [Required, StringLength(50), Display(Name = "Last Name")]
        public string ProcedureName { get; set; }
        public virtual ICollection<AppointmentModel> AppointmentModels { get; set; }

    }
}
