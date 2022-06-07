using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Stomatology3.Models
{
    public class ProcedureType
    {
        [Required, Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProcId { get; set; }
        [Required, StringLength(50), Display(Name = "Last Name")]
        public string ProcedureName { get; set; }
        public virtual ICollection<AppointmentModel> AppointmentModels { get; set; }

    }
}
