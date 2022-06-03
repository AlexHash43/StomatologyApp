using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Stomatology3.Models
{
    public class User : IdentityUser
    {
        [Required, StringLength(50), Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required, StringLength(50), Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required, StringLength(100), Display(Name = "Last Name")]
        public string FullName { get; set; }
        [Required, StringLength(100), Display(Name = "Address")]
        public string Address { get; set; }
        public virtual ICollection<AppointmentModel> AppointmentModels { get; set; }

        //public ICollection<AppointmentModel>? AppointmentModels { get; set; }
        //public byte[] PasswordHash { get; set; }
        //public byte[] PasswordSalt { get; set; }
    }
}
