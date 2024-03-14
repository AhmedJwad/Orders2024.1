using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Shared.Entities
{
    public class Category
    {
        public int Id { get; set; }

        [Display(Name = "Category")]
        [MaxLength(100, ErrorMessage = "The field {0} cannot be more than {1} characters.")]
        [Required(ErrorMessage = "The field {0} is required.")]
        public string Name { get; set; } = null!;
    }
}
