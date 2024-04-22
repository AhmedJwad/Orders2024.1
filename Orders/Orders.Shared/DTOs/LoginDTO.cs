using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Shared.DTOs
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "The field {0} is required.")]
        [EmailAddress(ErrorMessage = "You must enter a valid email.")]
        public string Email { get; set; } = null!;

        [Display(Name = "Password")]
        [Required(ErrorMessage = "The field {0} is required.")]
        [MinLength(6, ErrorMessage = "The {0} field must have at least {1} characters.")]
        public string Password { get; set; } = null!;

    }
}
