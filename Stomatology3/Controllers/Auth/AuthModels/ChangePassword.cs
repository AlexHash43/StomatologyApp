using System.ComponentModel.DataAnnotations;

namespace Stomatology3.Controllers.Auth.AuthModels
{
    public class ChangePassword
    {
        [Required(ErrorMessage = "UserName is required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Your current password")]
        public string CurrentPassword { get; set; }
        [Required(ErrorMessage = "New password")]
        public string NewPassword { get; set; } 
        [Required(ErrorMessage = "Confirm new password")]
        public string ConfirmPassword { get; set; }
    }
}
