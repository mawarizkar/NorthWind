using NorthWind.ViewModels.CustomValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthWind.ViewModels {
    public class RegisterVM {

        [Required]
        [StringLength(20)]
        [UniqueUsername]
        public string Username { get; set; }

        [Required]
        [StringLength(20)]
        public string Password { get; set; }

        [Required]
        [StringLength(20)]
        [Compare("Password", ErrorMessage = "Password not matched!")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Role { get; set; }
        public List<DropDownVM> RolesDropdown { get; set; }
    }
}
