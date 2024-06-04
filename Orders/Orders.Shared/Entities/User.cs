using Microsoft.AspNetCore.Identity;
using Orders.Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Shared.Entities
{
    public class User:IdentityUser
    {        

        [Display(Name = "First Name")]
        [MaxLength(50, ErrorMessage = "The field {0} must have a maximum of {1} characters.")]
        [Required(ErrorMessage = "The field {0} is required.")]
        public string FirstName { get; set; } = null!;

        [Display(Name = "Last Name")]
        [MaxLength(50, ErrorMessage = "The field {0} must have a maximum of {1} characters.")]
        [Required(ErrorMessage = "The field {0} is required.")]
        public string LastName { get; set; } = null!;

        [Display(Name = "Address")]
        [MaxLength(200, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Address { get; set; } = null!;

        [Display(Name = "Photo")]
        public string? Photo { get; set; }

        [Display(Name = "Type of user")]
        public UserType UserType { get; set; }

        public City? City { get; set; }

        [Display(Name = "City")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a {0}.")]
        public int CityId { get; set; }

        [Display(Name = "User")]
        public string FullName => $"{FirstName} {LastName}";
        public ICollection<TemporalOrder>? TemporalOrders { get; set; }
        public ICollection<Order>? Orders { get; set; }


    }
}
